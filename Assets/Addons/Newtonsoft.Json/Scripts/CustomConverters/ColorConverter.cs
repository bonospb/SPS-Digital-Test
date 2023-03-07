using System;
using UnityEngine;

namespace Newtonsoft.Json.CustomConverters
{
    public sealed class ColorConverter : JsonConverter<Color>
    {
        #region Public
        public override bool CanRead => true;
        public override bool CanWrite => true;
        #endregion

        #region Public methods
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            var s = $"{ColorUtility.ToHtmlStringRGBA(value)}";
            writer.WriteValue(s);
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var result = Color.clear;
            if (reader.Value != null)
                ColorUtility.TryParseHtmlString($"#{(string)reader.Value}", out result);

            return result;
        }
        #endregion
    }
}
