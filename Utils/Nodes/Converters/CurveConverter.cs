using DirN.Utils.NgManager.Curves;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Converters
{
    public class CurveConverter : JsonConverter<ICurve>
    {
        public override ICurve? ReadJson(JsonReader reader, Type objectType, ICurve? existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, ICurve? value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if(value is null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();
            if(value.Starter is not null && value.Ender is not null)
            {
                writer.WritePropertyName("Starter");
                writer.WriteValue(value.Starter.Id);
                writer.WritePropertyName("Ender");
                writer.WriteValue(value.Ender.Id);
            }
            writer.WriteEndObject();

        }
    }
}
