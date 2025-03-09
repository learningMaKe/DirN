
using DirN.Utils.NgManager;
using DirN.Utils.NgManager.Curves;
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
            Dictionary<Guid,IConnector> connectors = [];
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
                    Guid? guid = nodeObject["Guid"]?.ToObject<Guid>();

                    BaseNodeViewModel node = new(); 
                    if (position != null && handlerType != null && guid != null)
                    {
                        node.Position = position.Value;
                        node.HandlerType = handlerType.Value;
                        node.Id = guid.Value;
                    }
                    JArray? inputIds = (JArray?)nodeObject["InputIds"];
                    JArray? outputIds = (JArray?)nodeObject["OutputIds"];
                    JArray? inputData = (JArray?)nodeObject["InputData"];
                    if (inputIds != null)
                    {
                        for(int i = 0; i < inputIds.Count; i++)
                        {
                            Guid connectorId = Guid.Parse(inputIds[i].ToString());
                            connectors.Add(connectorId, node.Handler!.InputGroup[i].Connector);
                        }
                    }
                    if (outputIds != null)
                    {
                        for(int i = 0; i < outputIds.Count; i++)
                        {
                            Guid connectorId = Guid.Parse(outputIds[i].ToString());
                            connectors.Add(connectorId, node.Handler!.OutputGroup[i].Connector);
                        }
                    }
                    if (inputData != null)
                    {
                        for (int i = 0; i < inputData.Count; i++)
                        {
                            object d = System.Convert.ChangeType(inputData[i], node.Handler!.InputGroup[i].PointerType);
                            node.Handler.InputGroup[i].Data = d;
                        }
                    }

                }
            }

            JArray? jCurves = (JArray?)jObject["BezierCurves"];
            if (jCurves != null)
            {
                foreach (JObject curveObject in jCurves.Cast<JObject>())
                {
                    string? startIdStr= curveObject["Starter"]?.ToObject<string>();
                    if (string.IsNullOrEmpty(startIdStr)) continue;
                    Guid startId = Guid.Parse(startIdStr);
                    string? endIdStr = curveObject["Ender"]?.ToObject<string>();
                    if (string.IsNullOrEmpty(endIdStr)) continue;
                    Guid endId = Guid.Parse(endIdStr);
                    if(connectors.TryGetValue(startId, out IConnector? startConnector)&&connectors.TryGetValue(endId, out IConnector? endConnector))
                    {
                        BezierCurve curve = new()
                        {
                            Starter = startConnector,
                            Ender = endConnector
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

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(value.SWords));
            serializer.Serialize(writer, value.SWords);
            writer.WritePropertyName(nameof(value.Nodes));
            serializer.Serialize(writer, value.Nodes);
            writer.WritePropertyName(nameof(value.BezierCurves));
            serializer.Serialize(writer, value.BezierCurves);
            writer.WriteEndObject();
        }
    }
}
