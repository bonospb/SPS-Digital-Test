using System;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json.CustomConverters
{
    public class RangeConverter : JsonConverter<Range>
    {
        private const string pattern = @"\[(?<min>[^;]*); *(?<max>[^\]]*)\]";

        public override bool CanRead => true;
        public override bool CanWrite => true;


        public override void WriteJson(JsonWriter writer, Range value, JsonSerializer serializer) => 
            writer.WriteValue($"[{value.Min};{value.Max}]");

        public override Range ReadJson(JsonReader reader, Type objectType, Range existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var match = Regex.Match((string)reader.Value, pattern);
            if (match.Success)
            {
                return new Range
                {
                    Min = float.Parse(match.Groups["min"].Value),
                    Max = float.Parse(match.Groups["max"].Value)
                };
            }

            return default;
        }
    }
}