using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace FreeTeam.Editor.Build
{
    public static class BuildPostprocessor
    {
        #region Public static methods
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path) =>
            Debug.Log($"ClientVersion: {string.Join(".", EditorBuildTools.ClientVersion)}");
        #endregion
    }
}