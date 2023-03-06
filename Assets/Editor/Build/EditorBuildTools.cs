using FreeTeam.BP.SourceControl;
using System;
using UnityEditor;
using UnityEngine;

namespace FreeTeam.Editor.Build
{
    public static class EditorBuildTools
    {
        #region Public static
        public static string[] ClientVersion
        {
            get
            {
                string currentVersion = PlayerSettings.bundleVersion;
                string[] currentVersionSplit = currentVersion.Split('.');
                if (currentVersionSplit.Length < 3)
                    currentVersionSplit = new string[] { currentVersionSplit[0], currentVersionSplit[1], "0" };

                return currentVersionSplit;
            }
        }
        #endregion

        #region Public static methods
        [MenuItem("Tools/Build/Update Version", false, 9999)]
        public static void UpdateVersion()
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var currentVersion = ClientVersion;

            try
            {
                int versionMajor = Convert.ToInt32(currentVersion[0]);
                int versionMinor = Convert.ToInt32(currentVersion[1]);
                int versionMicro = new GitVersion().Version;

                PlayerSettings.bundleVersion = $"{versionMajor}.{versionMinor}.{versionMicro}";

                switch (buildTarget)
                {
                    case BuildTarget.iOS:
                        PlayerSettings.iOS.buildNumber = $"{versionMajor}{versionMinor}{versionMicro}";
                        Debug.Log($"BundleVersionCode: {PlayerSettings.iOS.buildNumber} and version {PlayerSettings.bundleVersion}");
                        break;
                    case BuildTarget.Android:
                        PlayerSettings.Android.bundleVersionCode = Convert.ToInt32($"{versionMajor}{versionMinor}{versionMicro}");
                        Debug.Log($"BundleVersionCode: {PlayerSettings.Android.bundleVersionCode} and version: {PlayerSettings.bundleVersion}");
                        break;
                    default:
                        Debug.LogWarning("Not supported Target Platform");
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\r\n{e}");
                Debug.LogError("AutoIncrementBuildVersion script failed.");
            }
        }
        #endregion
    }
}
