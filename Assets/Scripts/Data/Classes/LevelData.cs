using FreeTeam.BP.Configuration;
using System;

namespace FreeTeam.BP.Data
{
    public class LevelData
    {
        #region Public
        public LevelConfig LevelConfig { get; private set; }
        public uint LevelResults
        {
            get => _levelResults;
            set
            {
                if (_levelResults == value)
                    return;

                _levelResults = value;

                OnChanged?.Invoke();
            }
        }

        public event Action OnChanged = null;
        #endregion

        #region Private
        private uint _levelResults = 0;
        #endregion

        public LevelData(LevelConfig config) =>
            LevelConfig = config;
    }
}
