using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Newtonsoft.Json.CustomConverters
{
    public class Vector3Converter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector3 v = (Vector3)value;
            JToken t = JToken.FromObject(
                new Dictionary<string, object>() {
                    {"x", v.x},
                    {"y", v.y},
                    {"z", v.z}
                }
            );
            t.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(Vector3);
    }

    public class Vector3ListConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            List<Vector3> v = (List<Vector3>)value;
            var list = new List<Dictionary<string, object>>();
            foreach (var item in v)
            {
                list.Add(
                    new Dictionary<string, object>() {
                    {"x", item.x},
                    {"y", item.y},
                    {"z", item.z}
                });
            }
            JToken t = JToken.FromObject(list);
            t.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(List<Vector3>);
    }
}
