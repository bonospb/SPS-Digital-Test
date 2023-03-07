using System.IO;

namespace FreeTeam.BP.Editor.Common
{
    public static class Paths
    {
        #region Constants
        public const string CONFIG_FILENAME = "config";
        public static readonly string CONFIG_PATH = Path.Combine("Configs/", CONFIG_FILENAME);

        public const string PROFILE_FILENAME = "profile";
        public const string SETTINGS_FILENAME = "settings";
        #endregion
    }
}
