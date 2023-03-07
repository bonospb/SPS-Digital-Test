using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FreeTeam.BP.UI
{
    public class ExToggleGroup : MonoBehaviour
    {
        #region Internal
        [Serializable]
        public class TabChangedEvent : UnityEvent<ExToggleGroup> { }
        #endregion

        #region Serialize Fields
        [Header("Current toggle index:")]
        [SerializeField] private int index = -1;
        [Space]
        [SerializeField] [HideInInspector] private List<ExToggle> toggles = null;
        [SerializeField] [HideInInspector] private TabChangedEvent onChanged = new TabChangedEvent();
        #endregion

        #region Public
        public int Index
        {
            get { return index; }
            set
            {
                var newIndex = Mathf.Clamp(value, 0, Count - 1);

                if (index == newIndex)
                    return;

                index = newIndex;

                onChanged?.Invoke(this);

                RefreshTabs();
            }
        }

        public int Count => (toggles != null) ? toggles.Count : 0;

        public ExToggle CurrentToggledButton => (Count > 0 && Index > -1) ? toggles[Index] : null;

        public UnityEvent<ExToggleGroup> OnChanged => onChanged;
        #endregion

        #region Unity methods
#if UNITY_EDITOR
        private void OnValidate() { UnityEditor.EditorApplication.delayCall += _OnValidate; }
        protected virtual void _OnValidate()
        {
            if (Application.isPlaying)
                return;

            Index = (index == -1 && toggles != null && toggles.Count > 0) ? 0 : Index;

            UpdateListeners();
            RefreshTabs();
        }
#endif

        private void OnEnable() =>
            UpdateListeners();

        private void OnDisable() =>
            UpdateListeners();

        private void Start() =>
            RefreshTabs();
        #endregion

        #region Public methods
        public void SetIndex(int value) => Index = value;

        public void SetIndexWithoutNotify(int value)
        {
            var newIndex = Mathf.Clamp(value, 0, Count - 1);

            if (index == newIndex)
                return;

            index = newIndex;

            RefreshTabs();
        }

        public void AddToggle(ExToggle toggle)
        {
            if (!toggle)
                return;

            RemoveListeners();

            toggles.Add(toggle);

            AddListeners();
            RefreshTabs();
        }

        public void RemoveToggle(ExToggle toggle)
        {
            if (!toggles.Contains(toggle))
                return;

            RemoveListeners();

            toggles.Remove(toggle);

            AddListeners();

            SetIndexWithoutNotify(index);
        }

        public void ClearToggles()
        {
            if (toggles.Count <= 0)
                return;

            RemoveListeners();

            toggles.Clear();

            AddListeners();

            SetIndexWithoutNotify(-1);
        }

        public void NextTab() => Index++;

        public void PrevTab() => Index--;
        #endregion

        #region Private methods
        private void UpdateListeners()
        {
            if (Count <= 0)
                return;

            RemoveListeners();

            AddListeners();
        }

        private void AddListeners()
        {
            if (Count <= 0)
                return;

            foreach (var toggled in toggles)
            {
                if (toggled)
                    toggled.OnToggled.AddListener(OnToggledButtonHandler);
            }
        }

        private void RemoveListeners()
        {
            if (Count <= 0)
                return;

            foreach (var toggled in toggles)
            {
                if (toggled)
                    toggled.OnToggled.RemoveListener(OnToggledButtonHandler);
            }
        }

        private void RefreshTabs()
        {
            if (Count <= 0)
                return;

            foreach (var toggle in toggles)
            {
                if (toggle)
                    toggle.SetToggledWithoutNotify(CurrentToggledButton && toggle == CurrentToggledButton);
            }
        }

        private void OnToggledButtonHandler(IToggle toggleButton)
        {
            var newIndex = toggles.IndexOf((ExToggle)toggleButton);
            if (Index == newIndex)
                toggleButton.SetToggledWithoutNotify(true);

            Index = newIndex;
        }
        #endregion
    }
}
