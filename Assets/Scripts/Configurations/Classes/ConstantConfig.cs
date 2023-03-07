namespace FreeTeam.BP.Configuration
{
    [System.Serializable]
    public class ConstantConfig : IConfig
    {
        #region Public
        public string Id { get; private set; }
        public string Value { get; private set; }
        #endregion
    }
}
