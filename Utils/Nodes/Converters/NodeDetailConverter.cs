using DirN.Utils.NgManager;
using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes.Datas;
using DirN.ViewModels.Node;
using Fclp.Internals.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirN.Utils.Nodes.Converters
{
    public class NodeDetailConverter : JsonConverter<NodeDetail>
    {
        // ToDo: 记得测试
        public override NodeDetail? ReadJson(JsonReader reader, Type objectType, NodeDetail? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Dictionary<int,IConnector> connectors = [];
            NodeDetail nodeDetail = new();
            #if !OUT
            JObject jObject = JObject.Load(reader);
            JArray? jArray = (JArray?)jObject["SWords"];
            if (jArray!= null)
            {
                foreach (JObject wordObject in jArray.Cast<JObject>())
                {
                    string? word = (string?)wordObject["Word"];
                    int? index = (int?)wordObject["Index"];
                    if (word!= null && index.HasValue)
                    {
                        SWord sword = new() 
                        {
                            Word = word,
                            Index = index.Value
                        };
                        nodeDetail.SWords.Add(sword);
                    }
                }
            }
            Debug.WriteLine(string.Join("\n", [.. nodeDetail.SWords.Select(x => x.Word + " " + x.Index)]));
            
            JArray? jNodes = (JArray?)jObject["Nodes"];
            if (jNodes != null)
            {
                foreach (JObject nodeObject in jNodes.Cast<JObject>())
                {
                    Point? position = nodeObject["Position"]?.ToObject<Point>();
                    HandlerType? handlerType = nodeObject["HandlerType"]?.ToObject<HandlerType>();

                    BaseNodeViewModel node = new(); 
                    if (position != null && handlerType != null)
                    {
                        node.Position = position.Value;
                        node.HandlerType = handlerType.Value;
                    }
                    JArray? inputIds = (JArray?)nodeObject["InputIds"];
                    JArray? outputIds = (JArray?)nodeObject["OutputIds"];
                    JArray? inputData = (JArray?)nodeObject["InputData"];
                    if (inputIds != null)
                    {
                        for(int i = 0; i < inputIds.Count; i++)
                        {
                            int connectorId = int.Parse(inputIds[i].ToString());
                            connectors.Add(connectorId, node.Handler!.InputGroup[i].Connector);
                        }
                    }
                    if (outputIds != null)
                    {
                        for(int i = 0; i < outputIds.Count; i++)
                        {
                            int connectorId = int.Parse(outputIds[i].ToString());
                            connectors.Add(connectorId, node.Handler!.OutputGroup[i].Connector);
                        }
                    }
                    if (inputData != null)
                    {
                        for (int i = 0; i < inputData.Count; i++)
                        {
                            try
                            {
                                DataContainer? dc = serializer.Deserialize<DataContainer>(inputData[i].CreateReader());
                                if (dc != null)
                                {
                                    node.Handler!.InputGroup[i].Data = dc;
                                }
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }

                    nodeDetail.Nodes.Add(node);
                }
            }

            JArray? jCurves = (JArray?)jObject["BezierCurves"];
            if (jCurves != null)
            {
                foreach (JObject curveObject in jCurves.Cast<JObject>())
                {
                    string? startIdStr= curveObject["Starter"]?.ToObject<string>();
                    if (string.IsNullOrEmpty(startIdStr)) continue;
                    int startId = int.Parse(startIdStr);
                    string? endIdStr = curveObject["Ender"]?.ToObject<string>();
                    if (string.IsNullOrEmpty(endIdStr)) continue;
                    int endId = int.Parse(endIdStr);
                    if(connectors.TryGetValue(startId, out IConnector? startConnector)&&connectors.TryGetValue(endId, out IConnector? endConnector))
                    {
                        BezierCurve curve = new();
                        startConnector.LoadedCallback += () =>
                        {
                            curve.Starter = startConnector;
                        };
                        endConnector.LoadedCallback += () =>
                        {
                            curve.Ender = endConnector;
                        };
                        nodeDetail.BezierCurves.Add(curve);
                    }
                }
            }
            #else
            string json = string.Empty;
            while (reader.Read())
            {
                json+=reader.TokenType + " " + reader.Value + "\n";
            }
            using StreamWriter sw = new(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\NodeDetail.txt");
            sw.Write(json);
            #endif

            return nodeDetail;

        }

        public override void WriteJson(JsonWriter writer, NodeDetail? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            Dictionary<IConnector, int> connectorMap = [];

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(value.SWords));
            serializer.Serialize(writer, value.SWords);

            // Write Nodes
            writer.WritePropertyName(nameof(value.Nodes));
            writer.WriteStartArray();
            int connectorIndex = 0;
            foreach (var node in value.Nodes)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Position");
                serializer.Serialize(writer, node.Position);
                writer.WritePropertyName("HandlerType");
                writer.WriteValue(node.HandlerType.ToString());
                writer.WritePropertyName("InputData");
                writer.WriteStartArray();
                foreach (var data in node.InputDataGroup)
                {
                    serializer.Serialize(writer, data);
                }
                writer.WriteEndArray();
                writer.WritePropertyName("InputIds");
                writer.WriteStartArray();
                foreach (var ic in node.InputConnectors)
                {
                    writer.WriteValue(connectorIndex);
                    connectorMap.Add(ic, connectorIndex);
                    connectorIndex++;
                }
                writer.WriteEndArray();
                writer.WritePropertyName("OutputIds");
                writer.WriteStartArray();
                foreach (var oc in node.OutputConnectors)
                {
                    writer.WriteValue(connectorIndex);
                    connectorMap.Add(oc, connectorIndex);
                    connectorIndex++;
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            // Write BezierCurves
            writer.WritePropertyName(nameof(value.BezierCurves));
            writer.WriteStartArray();
            foreach (var curve in value.BezierCurves)
            {
                writer.WriteStartObject();
                if (curve.Starter is not null && curve.Ender is not null)
                {
                    writer.WritePropertyName("Starter");
                    connectorMap.TryGetValue(curve.Starter, out int si);
                    writer.WriteValue(si);
                    writer.WritePropertyName("Ender");
                    connectorMap.TryGetValue(curve.Ender, out int ei);
                    writer.WriteValue(ei);
                }
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
