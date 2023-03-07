using FreeTeam.BP.Configuration;
using System;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json.CustomConverters
{
    public class EnemyConfigConverter : JsonConverter<EnemyConfig>
    {
        private const string pattern = @"(?<id>[\w]*)(:(?<count>[\d]+)(:(?<limit>[\d]+))?)?";

        public override bool CanRead => true;
        public override bool CanWrite => true;


        public override void WriteJson(JsonWriter writer, EnemyConfig value, JsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(value.Id))
                writer.WriteNull();
            else
                writer.WriteValue($"{value.Id}:{value.Count}:{value.Limit}");
        }

        public override EnemyConfig ReadJson(JsonReader reader, Type objectType,
            EnemyConfig existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return default;

            var match = Regex.Match((string)reader.Value, pattern);

            var id = match.Groups["id"].Value;

            if (!uint.TryParse(match.Groups["count"].Value, out var count))
                count = 1;

            if (!uint.TryParse(match.Groups["limit"].Value, out var limit))
                limit = 1;

            return new EnemyConfig(id, count, limit);
        }
    }
}