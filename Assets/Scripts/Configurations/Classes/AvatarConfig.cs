namespace FreeTeam.BP.Configuration
{
    public class AvatarConfig : IConfig
    {
        #region Public
        public string Id { get; private set; }

        public string PrefabPath { get; private set; }

        public string[] AvailableArms { get; private set; }

        public int Health { get; private set; }
        #endregion
    }
}
