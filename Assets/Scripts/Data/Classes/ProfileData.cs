using FreeTeam.BP.Common;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace FreeTeam.BP.Data
{
    public class ProfileData : IDirty
    {
        #region Public
        public Guid UID { get; private set; }

        public DateTime Created { get; private set; }
        public DateTime Updated { get; private set; }

        public ProgressData ProgressData { get; private set; }

        [JsonIgnore]
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                if (_isDirty == value)
                    return;

                _isDirty = value;

                Updated = DateTime.UtcNow;

                OnProfileChanged?.Invoke();
            }
        }

        public event Action OnProfileChanged = null;
        #endregion

        #region Private
        private bool _isDirty = true;
        #endregion

        #region Constructor/Destructor
        public ProfileData()
        {
            UID = Guid.NewGuid();

            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;

            ProgressData = new ProgressData();

            AddListeners();
        }

        ~ProfileData()
        {
            RemoveListeners();
        }
        #endregion

        #region Internal methods
        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context) =>
            AddListeners();
        #endregion

        #region Public methods
        public void SetDirtyWithoutNotify(bool value)
        {
            _isDirty = value;

            ProgressData.SetDirtyWithoutNotify(_isDirty);
        }
        #endregion

        #region Private methods
        private void AddListeners()
        {
            ProgressData.OnProgressChanged += OnProgressDataChangedHandler;
        }

        private void RemoveListeners()
        {
            ProgressData.OnProgressChanged += OnProgressDataChangedHandler;
        }

        private void OnProgressDataChangedHandler() =>
            IsDirty = true;
        #endregion
    }
}
