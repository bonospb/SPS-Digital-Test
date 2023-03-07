using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.CustomConverters;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace FreeTeam.BP.Extensions
{
    public static class SerializeExtensions
    {
        #region Internal
        private class NonPublicPropertiesResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
            {
                var prop = base.CreateProperty(member, memberSerialization);
                if (!prop.Writable)
                {
                    var property = member as System.Reflection.PropertyInfo;
                    var hasPrivateSetter = property?.GetSetMethod(true) != null;
                    prop.Writable = hasPrivateSetter;
                }
                return prop;
            }
        }
        #endregion

        #region Static private
        private static readonly JsonSerializerSettings SerializeSettings;
        private static readonly JsonSerializerSettings DeserializeSettings;
        #endregion

        static SerializeExtensions()
        {
            SerializeSettings = new JsonSerializerSettings
            {
                Error = delegate (object sender, ErrorEventArgs args)
                {
                    Debug.LogError(args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                },
            };
            SerializeSettings.Converters.Add(new ProgressDataConverter());
            SerializeSettings.Converters.Add(new ProgressItemConverter());
            SerializeSettings.Converters.Add(new EnemyConfigConverter());
            SerializeSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeStyles = System.Globalization.DateTimeStyles.AdjustToUniversal, DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'" });

            DeserializeSettings = new JsonSerializerSettings
            {
                ContractResolver = new NonPublicPropertiesResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                NullValueHandling = NullValueHandling.Ignore,
                Error = delegate (object sender, ErrorEventArgs args)
                {
                    Debug.LogError(args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                },
            };
            DeserializeSettings.Converters.Add(new ProgressDataConverter());
            DeserializeSettings.Converters.Add(new ProgressItemConverter());
            DeserializeSettings.Converters.Add(new EnemyConfigConverter());
            DeserializeSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeStyles = System.Globalization.DateTimeStyles.AssumeUniversal });
        }

        #region Extensions
        public static string Serialize(this object target, bool indented = false)
        {
            var formatting = (indented) ? Formatting.Indented : Formatting.None;

            return JsonConvert.SerializeObject(target, formatting, SerializeSettings);
        }

        public static T Deserialize<T>(this string target) =>
            JsonConvert.DeserializeObject<T>(target, DeserializeSettings);
        #endregion
    }
}
