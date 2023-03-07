namespace FreeTeam.BP.Configuration
{
    public class EnemyConfig : IConfig
    {
        #region Public
        public string Id { get; private set; }
        public uint Count { get; private set; }
        public uint Limit { get; private set; }
        #endregion

        public EnemyConfig(string id, uint count, uint limit)
        {
            Id = id;
            Count = count;
            Limit = limit;
        }
    }
}
