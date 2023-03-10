using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Newtonsoft.Json.CustomConverters
{
	public class Vector4Converter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JToken t = JToken.FromObject(value);

			if (t.Type != JTokenType.Object)
			{
				t.WriteTo(writer);
			}
			else
			{
				var o = (JObject)t;
				IList<string> propertyNames = o
					.Properties()
					.Where(p => p.Name == "w" || p.Name == "x" || p.Name == "y" || p.Name == "z")
					.Select(p => p.Name)
					.ToList();

				o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));
				o.WriteTo(writer);
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
			throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");

		public override bool CanRead => false;

		public override bool CanConvert(Type objectType) =>
			objectType == typeof(Vector4);
	}
}
