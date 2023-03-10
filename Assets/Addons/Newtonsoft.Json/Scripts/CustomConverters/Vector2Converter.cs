using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Newtonsoft.Json.CustomConverters
{
    public class Vector2Converter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector2 v = (Vector2)value;
            JToken t = JToken.FromObject(
                new Dictionary<string, object>() {
                    {"x", v.x},
                    {"y", v.y}
                }
            );
            t.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(Vector2);
    }
}
