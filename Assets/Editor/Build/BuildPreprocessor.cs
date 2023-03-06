using UnityEditor.Build.Reporting;
using UnityEditor.Build;

namespace FreeTeam.Editor.Build
{
    public class BuildPreprocessor : IPreprocessBuildWithReport
    {
        #region Public
        public int callbackOrder { get; }
        #endregion

        #region Public methods
        public void OnPreprocessBuild(BuildReport report) =>
            EditorBuildTools.UpdateVersion();
        #endregion
    }
}