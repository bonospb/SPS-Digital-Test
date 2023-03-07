using FreeTeam.BP.Configuration;
using FreeTeam.BP.Editor.Common;
using FreeTeam.BP.Extensions;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FreeTeam.BP.Editor.Tools
{
    public static class EditorConfigTools
    {
        #region Public static methods
        //[MenuItem("Tools/Config/Encrypt configuration", false, 100)]
        public static void EncryptConfiguration()
        {
            var openPath = EditorUtility.OpenFilePanel("Open decrypted json config file...", "Assets", "json");
            if (string.IsNullOrEmpty(openPath))
                return;

            var contents = File.ReadAllText(openPath);
            var config = Configurations.Load(contents);

            if (!CheckConfig(config))
            {
                Debug.LogError("CheckConfig() failed!");
                return;
            }

            var json = config.Serialize();

            var savePath = EditorUtility.SaveFilePanel("Save encrypted config file...", Path.GetDirectoryName(openPath), Paths.CONFIG_FILENAME, "");
            if (string.IsNullOrEmpty(savePath))
                return;

            File.WriteAllText(savePath, json);

            AssetDatabase.Refresh();

            Debug.Log($"Encrypt Configuration file complete!");
        }

        //[MenuItem("Tools/Config/Decrypt configuration", false, 101)]
        public static void DecryptConfiguration()
        {
            var openPath = EditorUtility.OpenFilePanel("Open encrypted config file...", "Assets", "");
            if (string.IsNullOrEmpty(openPath))
                return;

            if (!File.Exists(openPath))
            {
                Debug.LogError($"File <b><i>{openPath}</i></b> not found!");
                return;
            }

            var contents = File.ReadAllText(openPath);
            var config = contents.Deserialize<Configurations>();

            if (!CheckConfig(config))
            {
                Debug.LogError("CheckConfig() failed!");
                return;
            }

            var json = config.Serialize();

            var destinationPath = EditorUtility.SaveFilePanel("Save decrypted json config file...", Path.GetDirectoryName(openPath), $"{Paths.CONFIG_FILENAME}.json", "json");
            if (string.IsNullOrEmpty(destinationPath))
                return;

            File.WriteAllText(destinationPath, json);

            Debug.Log("Decrypt Configuration file complete");

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Config/Encrypt configuration and paste into project", false, 102)]
        public static void EncryptConfigurationAndPasteInfoResFolder()
        {
            var path = EditorUtility.OpenFilePanel("Open unencrypted config file...", "Assets", "json");
            if (!string.IsNullOrEmpty(path))
            {
                var contents = File.ReadAllText(path);
                var config = contents.Deserialize<Configurations>();

                if (!CheckConfig(config))
                {
                    Debug.LogError("CheckConfig() failed!");
                    return;
                }

                var json = config.Serialize();

                File.WriteAllText(Path.Combine(Application.dataPath, "Bundles", Paths.CONFIG_PATH + ".txt"), json);

                AssetDatabase.Refresh();

                Debug.Log("Encrypt configuration and paste into project complete.");
            }
        }

        [MenuItem("Tools/Config/Decrypt configuration from ResFolder", false, 103)]
        public static void DecryptConfigurationFromResFolder()
        {
            var path = Path.Combine(Application.dataPath, "Bundles", Paths.CONFIG_PATH + ".txt");
            if (string.IsNullOrEmpty(path))
                return;

            if (!File.Exists(path))
            {
                Debug.LogError($"File <b><i>{path}</i></b> not found!");
                return;
            }

            var contents = File.ReadAllText(path);
            var config = contents.Deserialize<Configurations>();

            if (!CheckConfig(config))
            {
                Debug.LogError("CheckConfig() failed!");
                return;
            }

            var json = config.Serialize(true);

            var destinationPath = EditorUtility.SaveFilePanel("Save decrypted config file...", Application.dataPath, "config.json", "json");
            if (string.IsNullOrEmpty(destinationPath))
                return;

            File.WriteAllText(destinationPath, json);

            Debug.Log("Decrypt configuration from ResFolder complete.");

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Config/Copy config to AppDataPath", false, 120)]
        public static void CopyConfigurationToAppDataPath()
        {
            var from = Path.Combine(Application.dataPath, "Bundles", Paths.CONFIG_PATH + ".txt");
            var to = Path.Combine(Application.persistentDataPath, Paths.CONFIG_FILENAME);
            File.Copy(from, to, true);

            Debug.Log($"Config file copied from <b><i>{from}</i></b> to <b><i>{to}</i></b>");
        }

        [MenuItem("Tools/Config/Remove config from AppDataPath", false, 121)]
        public static void RemoveConfigurationFromAppDataPath()
        {
            var path = Path.Combine(Application.persistentDataPath, Paths.CONFIG_FILENAME);
            if (File.Exists(path))
                File.Delete(path);

            Debug.Log("Configuration file removed!");
        }
        #endregion

        #region Private static methods
        private static bool CheckConfig(Configurations config)
        {
            bool result = config != null;

            return result;
        }
        #endregion
    }
}
