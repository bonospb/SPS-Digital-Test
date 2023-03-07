namespace FreeTeam.BP.Configuration
{
    public class LevelConfig : IConfig
    {
        #region Public
        public string Id { get; private set; }

        public string Scene { get; private set; }

        public EnemyConfig[] Enemies { get; private set; }
        #endregion
    }
}
