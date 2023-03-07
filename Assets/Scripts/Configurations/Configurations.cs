using FreeTeam.BP.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeTeam.BP.Configuration
{
    public sealed class Configurations
    {
        #region Public
        [JsonProperty("Constants")] private ConstantConfig[] _constants = Array.Empty<ConstantConfig>();
        [JsonProperty("Levels")] private LevelConfig[] _levels = Array.Empty<LevelConfig>();
        [JsonProperty("Vehicles")] private AvatarConfig[] _avatars = Array.Empty<AvatarConfig>();
        [JsonProperty("Arms")] private ArmConfig[] _arms = Array.Empty<ArmConfig>();

        [JsonIgnore] public Dictionary<string, string> Constants { get; private set; }
        [JsonIgnore] public Dictionary<string, LevelConfig> Levels { get; private set; }
        [JsonIgnore] public Dictionary<string, AvatarConfig> Avatars { get; private set; }
        [JsonIgnore] public Dictionary<string, ArmConfig> Arms { get; private set; }
        #endregion

        #region Public methods
        public void Optimize()
        {
            Constants = _constants.ToDictionary(x => x.Id, y => y.Value);
            Levels = _levels.ToDictionary(x => x.Id, y => y);
            Avatars = _avatars.ToDictionary(x => x.Id, y => y);
            Arms = _arms.ToDictionary(x => x.Id, y => y);
        }
        #endregion

        #region Public static methods
        public static Configurations Load(string configContents)
        {
            var configuration = configContents.Deserialize<Configurations>();
            configuration.Optimize();

            return configuration;
        }
        #endregion
    }
}
