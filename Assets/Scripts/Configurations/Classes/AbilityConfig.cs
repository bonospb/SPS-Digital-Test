﻿namespace FreeTeam.BP.Configuration
{
    public class AbilityConfig : IConfig
    {
        #region Public
        public string Id { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public bool IsNegative { get; private set; }
        public bool FriendlyFire { get; private set; }

        public float Cooldown { get; private set; }
        public float Value { get; private set; }

        public string Upgrade { get; private set; }
        #endregion
    }
}