using FreeTeam.BP.Data;
using Newtonsoft.Json.Linq;
using System;

namespace Newtonsoft.Json.CustomConverters
{
    public class ProgressItemConverter : Converters.CustomCreationConverter<IProgressItem>
    {
        public override IProgressItem Create(Type objectType) =>
            throw new NotImplementedException();

        public IProgressItem Create(Type objectType, JObject jObject)
        {
            var type = (string)jObject.Property("Type");

            return (IProgressItem)Activator.CreateInstance(Type.GetType(type));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            base.WriteJson(writer, value, serializer);

            writer.WriteValue(((IProgressItem)value).Type);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            var target = Create(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
