using System;

namespace FreeTeam.BP.Data
{
    public class ProgressItem : IProgressItem
    {
        #region Public
        public string LevelId { get; private set; }

        public string Type => GetType().FullName;

        public uint Result
        {
            get => _result;
            set
            {
                if (_result == value)
                    return;

                _result = value;

                OnChanged();
            }
        }

        public event Action OnChanged = null;
        #endregion

        #region Private
        private uint _result = 0;
        #endregion

        public ProgressItem(string levelId) =>
            LevelId = levelId;
    }
}
