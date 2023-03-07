using FreeTeam.BP.Common;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FreeTeam.BP.Editor.Tools
{
    public static class EditorSavesTools
    {
        [MenuItem("Tools/Save/Open AppDataPath", false, 300)]
        public static void OpenPathSaveData()
        {
            Debug.Log(Application.persistentDataPath);

            var path = Application.persistentDataPath.Replace(@"/", @"\");
            System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
        }

        [MenuItem("Tools/Save/Remove AppDataPath", false, 301)]
        public static void RemoveSaveFolder()
        {
            if (Directory.Exists(Application.persistentDataPath))
                Directory.Delete(Application.persistentDataPath);

            Debug.Log($"AppDataPath ({Application.persistentDataPath}) is removed!");
        }

        [MenuItem("Tools/Save/Remove local Profile files", false, 302)]
        public static void RemoveSave()
        {
            var profilePath = Path.Combine(Application.persistentDataPath, Paths.PROFILE_PATH);
            var profileBackupPath = Path.Combine(Application.persistentDataPath, Paths.PROFILE_BACKUP_PATH);

            if (File.Exists(profilePath))
                File.Delete(profilePath);

            if (File.Exists(profileBackupPath))
                File.Delete(profileBackupPath);

            Debug.Log("Profile files removed!");
        }
    }
}
