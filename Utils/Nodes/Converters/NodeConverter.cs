using DirN.ViewModels.Node;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Converters
{
    public class NodeConverter : JsonConverter<INode>
    {
        public override INode? ReadJson(JsonReader reader, Type objectType, INode? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            while (reader.Read())
            {
                Debug.WriteLine(reader.Value);
            }
            return new BaseNodeViewModel() { HandlerType = HandlerType.Output };
        }

        public override void WriteJson(JsonWriter writer, INode? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();
            writer.WritePropertyName("Position");
            serializer.Serialize(writer, value.Position);
            writer.WritePropertyName("HandlerType");
            writer.WriteValue(value.HandlerType.ToString());
            writer.WritePropertyName("Guid");
            writer.WriteValue(value.Id);
            writer.WritePropertyName("InputData");
            writer.WriteStartArray();
            foreach(var data in value.InputDataGroup)
            {
                serializer.Serialize(writer, data);
            }
            writer.WriteEndArray();
            writer.WritePropertyName("InputIds");
            writer.WriteStartArray();
            foreach(var id in value.InputIds)
            {
                writer.WriteValue(id);
            }
            writer.WriteEndArray();
            writer.WritePropertyName("OutputIds");
            writer.WriteStartArray();
            foreach(var id in value.OutputIds)
            {
                writer.WriteValue(id);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();

        }
    }
}
