using UnityEngine;

namespace FreeTeam.BP.Configuration
{
    public static class ConfigurationsExtensions
    {
        #region Constants
        public static string GetConstantString(this Configurations config, string item, string defaultValue = null)
        {
            if (config.Constants.TryGetValue(item, out var result))
                return result;

            Debug.LogWarning($"Configuration constant <b>\"{item}\"</b> not found!");

            return defaultValue;
        }

        public static string[] GetConstantsArray(this Configurations config, string item)
        {
            var configStringValue = config.GetConstantString(item);
            if (configStringValue == null)
                return new string[0];

            return configStringValue.Trim().Split(',');
        }

        public static float[] GetConstantArrayFloat(this Configurations config, string item)
        {
            var array = config.GetConstantsArray(item);
            var resultArray = new float[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                float.TryParse(array[i], out var result);
                resultArray[i] = result;
            }

            return resultArray;
        }

        public static float GetConstantFloat(this Configurations config, string item, float defaultValue = 0f)
        {
            var configStringValue = config.GetConstantString(item);
            if (string.IsNullOrEmpty(configStringValue))
                return defaultValue;

            if (!float.TryParse(configStringValue, out var result))
                result = defaultValue;

            return result;
        }

        public static int GetConstantInt(this Configurations config, string item, int defaultValue = 0)
        {
            var configStringValue = config.GetConstantString(item);
            if (string.IsNullOrEmpty(configStringValue))
                return defaultValue;

            if (!int.TryParse(configStringValue, out var result))
                result = defaultValue;

            return result;
        }

        public static uint GetConstantUint(this Configurations config, string item, uint defaultValue = 0)
        {
            var configStringValue = config.GetConstantString(item);
            if (string.IsNullOrEmpty(configStringValue))
                return defaultValue;

            if (!uint.TryParse(configStringValue, out var result))
                result = defaultValue;

            return result;
        }

        public static bool GetConstantBool(this Configurations config, string item, bool defaultValue = false)
        {
            var configStringValue = config.GetConstantString(item);
            if (string.IsNullOrEmpty(configStringValue))
                return defaultValue;

            if (!bool.TryParse(configStringValue, out var result))
                result = defaultValue;

            return result;
        }
        #endregion
    }
}
