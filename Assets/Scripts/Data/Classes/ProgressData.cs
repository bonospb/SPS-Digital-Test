using FreeTeam.BP.Common;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FreeTeam.BP.Data
{
    public class ProgressData : IList<IProgressItem>, IDirty
    {
        #region Public
        [JsonIgnore]
        public int Count => _items.Count;

        [JsonIgnore]
        public bool IsReadOnly => false;

        [JsonIgnore]
        public IProgressItem this[int index]
        {
            get => _items[index];
            set
            {
                _items[index] = value;

                IsDirty = true;
            }
        }

        [JsonIgnore]
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                if (_isDirty == value)
                    return;

                _isDirty = value;

                OnProgressChanged?.Invoke();
            }
        }

        public event Action OnProgressChanged = null;
        #endregion

        #region Private
        private readonly List<IProgressItem> _items = new List<IProgressItem>();

        private bool _isDirty = true;
        #endregion

        #region Constructor/Destructor
        public ProgressData() =>
            AddListeners();

        public ProgressData(IEnumerable<IProgressItem> collection)
        {
            _items = collection.ToList();

            AddListeners();
        }

        ~ProgressData() =>
        RemoveListeners();
        #endregion

        #region Internal methods
        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context) =>
            AddListeners();
        #endregion

        #region Public methods
        public void SetDirtyWithoutNotify(bool value) =>
            _isDirty = value;
        #endregion

        #region Private methods
        private void AddListeners()
        {
            foreach (var item in _items)
                item.OnChanged += OnItemChangedHandler;
        }

        private void RemoveListeners()
        {
            foreach (var item in _items)
                item.OnChanged -= OnItemChangedHandler;
        }

        private void OnItemChangedHandler() =>
            IsDirty = true;
        #endregion

        #region Implementation
        public IEnumerator<IProgressItem> GetEnumerator() =>
            _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public int IndexOf(IProgressItem item) =>
            _items.IndexOf(item);

        public void Add(IProgressItem item)
        {
            _items.Add(item);

            IsDirty = true;
        }

        public void Insert(int index, IProgressItem item)
        {
            _items.Insert(index, item);

            IsDirty = true;
        }

        public bool Remove(IProgressItem item)
        {
            var results = _items.Remove(item);

            IsDirty = true;

            return results;
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);

            IsDirty = true;
        }
        public void Clear()
        {
            _items.Clear();

            IsDirty = true;
        }

        public bool Contains(IProgressItem item) =>
            _items.Contains(item);

        public void CopyTo(IProgressItem[] array, int arrayIndex) =>
            _items.CopyTo(array, arrayIndex);
        #endregion
    }
}
