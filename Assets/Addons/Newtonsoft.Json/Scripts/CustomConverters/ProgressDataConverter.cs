using FreeTeam.BP.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.CustomConverters
{
    public class ProgressDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(ProgressData);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JArray ja = JArray.FromObject(value);
            ja.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var array = serializer.Deserialize<IEnumerable<IProgressItem>>(reader);
            return new ProgressData(array);
        }
    }
}
