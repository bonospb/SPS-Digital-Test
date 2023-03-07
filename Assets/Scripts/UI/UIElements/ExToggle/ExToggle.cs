using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FreeTeam.BP.UI
{
    [RequireComponent(typeof(ExButton))]
    public class ExToggle : MonoBehaviour, IToggle
    {
        #region Internal
        [Serializable]
        public class ToggleChangedEvent : UnityEvent<IToggle> { }
        #endregion

        #region Serialize Fields
        [SerializeField] private ExButton button = null;
        [Space]
        [SerializeField] private bool toggled = true;
        [Space]
        [SerializeField] private ToggleChangedEvent onToggledChanged = new ToggleChangedEvent();
        [Space]
        [SerializeField] [HideInInspector] private List<GameObject> groupOn = null;
        [SerializeField] [HideInInspector] private List<GameObject> groupOff = null;
        #endregion

        #region Unity methods
#if UNITY_EDITOR
        private void OnValidate() { UnityEditor.EditorApplication.delayCall += _OnValidate; }
        protected virtual void _OnValidate() =>
            RefreshUI();
#endif

        private void OnEnable()
        {
            if (button)
                button.OnClick.AddListener(OnButtonClickHandler);
        }

        private void OnDisable()
        {
            if (button)
                button.OnClick.RemoveListener(OnButtonClickHandler);
        }

        private void Start() =>
            RefreshUI();
        #endregion

        #region Public
        public bool Toggled
        {
            get { return toggled; }
            set
            {
                toggled = value;

                OnToggled?.Invoke(this);

                RefreshUI();
            }
        }

        public bool Interactable
        {
            get => button.interactable;
            set => button.interactable = value;
        }

        public UnityEvent<IToggle> OnToggled => onToggledChanged;
        #endregion

        #region Public methods
        public void SetToggledWithoutNotify(bool value)
        {
            toggled = value;

            RefreshUI();
        }

        public void SetToggled(bool _value) => Toggled = _value;

        public void SetInteractable(bool _value) => Interactable = _value;
        #endregion

        #region Private methods
        private void OnButtonClickHandler(Selectable selectable) => Toggled = !Toggled;

        private void RefreshUI()
        {
            if (groupOn != null && groupOn.Count > 0)
            {
                foreach (var item in groupOn)
                {
                    if (item == null)
                        continue;

                    item.SetActive(Toggled);
                }
            }

            if (groupOff != null && groupOff.Count > 0)
            {
                foreach (var item in groupOff)
                {
                    if (item == null)
                        continue;

                    item.SetActive(!Toggled);
                }
            }
        }
        #endregion
    }
}
