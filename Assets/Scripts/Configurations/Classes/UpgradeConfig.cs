namespace FreeTeam.BP.Configuration
{
    public class UpgradeConfig : IConfig
    {
        #region Public
        public string Id { get; private set; }

        public float CooldownStep { get; private set; }
        public float ValueStep { get; private set; }

        public int BasePrice { get; private set; }

        public int PriceStep { get; private set; }
        #endregion
    }
}