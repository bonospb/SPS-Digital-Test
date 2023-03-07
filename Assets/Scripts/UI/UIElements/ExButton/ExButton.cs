using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FreeTeam.BP.UI
{
    public class ExButton : Selectable, IPointerClickHandler, ISubmitHandler, IEventSystemHandler
    {
        #region InternalTypes
        [Serializable]
        public class StateInfo
        {
            #region SerializeFields
            [SerializeField] private float transitionDuration = 0f;
            [SerializeField] private Vector2 offset = Vector2.zero;
            #endregion

            #region Public
            public float TransitionDuration => transitionDuration;
            public Vector2 Offset => offset;
            #endregion
        }

        [Serializable]
        public class BaseButtonElement
        {
            #region SerializeFields
            [SerializeField] private Graphic graphic = null;

            [SerializeField] private Color normalColor = Color.white;
            [SerializeField] private Color pressedColor = Color.white;
            [SerializeField] private Color disabledColor = Color.white;
            [SerializeField] private Color selectedColor = Color.white;
            [SerializeField] private Color highlightedColor = Color.white;

            [SerializeField] private bool normalActive = true;
            [SerializeField] private bool pressedActive = true;
            [SerializeField] private bool disableActive = true;
            [SerializeField] private bool selectedActive = true;
            [SerializeField] private bool highlightedActive = true;
            #endregion

            #region Public
            public Graphic Graphic => graphic;

            public Color NormalColor => normalColor;
            public Color PressedColor => pressedColor;
            public Color DisabledColor => disabledColor;
            public Color SelectedColor => selectedColor;
            public Color HighlightedColor => highlightedColor;

            public bool NormalActive => normalActive;
            public bool PressedActive => pressedActive;
            public bool DisableActive => disableActive;
            public bool SelectedActive => selectedActive;
            public bool HighlightedActive => highlightedActive;
            #endregion
        }

        [Serializable]
        private class ButtonElement : BaseButtonElement
        {
            #region Public methods
            public Color GetColor(SelectionState state)
            {
                Color result;

                switch (state)
                {
                    case SelectionState.Normal:
                        result = NormalColor;
                        break;
                    case SelectionState.Pressed:
                        result = PressedColor;
                        break;
                    case SelectionState.Highlighted:
                        result = HighlightedColor;
                        break;
                    case SelectionState.Selected:
                        result = SelectedColor;
                        break;
                    case SelectionState.Disabled:
                    default:
                        result = DisabledColor;
                        break;
                }

                return result;
            }
            #endregion
        }

        [Serializable]
        private class UnityEventSelectable : UnityEvent<Selectable> { }
        #endregion

        #region SerializeFields
        [SerializeField] private string id = null;

        [SerializeField] private StateInfo normal = null;
        [SerializeField] private StateInfo pressed = null;
        [SerializeField] private StateInfo selected = null;
        [SerializeField] private StateInfo highlighted = null;
        [SerializeField] private StateInfo disabled = null;

        [SerializeField] [HideInInspector] private ButtonElement[] elements = null;

        [Header("Events")]
        [SerializeField] private UnityEventSelectable onClick = new UnityEventSelectable();
        [SerializeField] private UnityEventSelectable onSelected = new UnityEventSelectable();
        [SerializeField] private UnityEventSelectable onDeselected = new UnityEventSelectable();
        #endregion

        #region Public
        public UnityEvent<Selectable> OnClick => onClick;
        public UnityEvent<Selectable> OnSelected => onSelected;
        public UnityEvent<Selectable> OnDeselected => onDeselected;

        public string Id
        {
            get => id;
            set => id = value;
        }
        #endregion 

        #region Private
        private SelectionState state = SelectionState.Normal;
        #endregion

        #region Unity methods
        protected override void Awake()
        {
            state = SelectionState.Normal;
            if (elements != null)
                foreach (var element in elements)
                {
                    if (element != null && element.Graphic)
                        element.Graphic.color = element.GetColor(state);
                }

            base.Awake();
        }
        #endregion

        #region Public methods
        public void SetActiveElement(string elementName, bool actirve)
        {
            foreach (var element in elements)
            {
                if (element.Graphic.gameObject.name == elementName)
                    element.Graphic.gameObject.SetActive(actirve);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Press();

            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);

            StartCoroutine(OnFinishSubmit());
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            onSelected?.Invoke(this);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            onDeselected?.Invoke(this);
        }
        #endregion

        #region Protected methods
        protected override void DoStateTransition(SelectionState newState, bool instant)
        {
            if (newState != state)
            {
                ChangeState(newState);
                base.DoStateTransition(newState, instant);
            }
        }
        #endregion

        #region Private methods
        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button.onClick", this);

            OnClick?.Invoke(this);
        }

        private void ChangeState(SelectionState newState)
        {
            if (newState != state)
            {
                var stateInfo = GetStateInfo(state);
                var newStateInfo = GetStateInfo(newState);

                foreach (var element in elements)
                {
                    element?.Graphic?.DOComplete();
                    element?.Graphic?.DOColor(element.GetColor(newState), newStateInfo.TransitionDuration);

                    if (stateInfo.Offset != Vector2.zero)
                        element.Graphic.rectTransform.anchoredPosition -= stateInfo.Offset;

                    if (newStateInfo.Offset != Vector2.zero)
                        element.Graphic.rectTransform.anchoredPosition += newStateInfo.Offset;
                }

                state = newState;
            }
        }

        private StateInfo GetStateInfo(SelectionState state)
        {
            switch (state)
            {
                case SelectionState.Normal: return normal;
                case SelectionState.Pressed: return pressed;
                case SelectionState.Selected: return selected;
                case SelectionState.Highlighted: return highlighted;
                case SelectionState.Disabled: default: return disabled;
            }
        }
        #endregion

        #region Coroutines
        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }
        #endregion
    }
}
