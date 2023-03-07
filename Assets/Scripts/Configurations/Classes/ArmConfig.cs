namespace FreeTeam.BP.Configuration
{
    public class ArmConfig : IConfig
    {
        #region Public
        public string Id { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public string PrefabPath { get; private set; }

        public float Cooldown { get; private set; }
        public float RotationSpeed { get; private set; }

        public string Ability { get; private set; }
        #endregion
    }
}