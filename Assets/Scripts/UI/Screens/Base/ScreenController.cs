using FreeTeam.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTeam.BP.UI.Screens
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ScreenController : UIController, IUIData, IUIResult, IUIPreviousControl
    {
        #region Internal
        public class ScreenData : IUIData, IUIPreviousControl
        {
            #region Public
            public object Data { get; private set; }
            public string PreviousControlName { get; protected set; }
            #endregion

            public ScreenData(object data, string prevScreenName)
            {
                Data = data;
                PreviousControlName = prevScreenName;
            }
        }
        #endregion

        #region SerializeFields
        [SerializeField] private bool dontShowOnInit = false;
        #endregion

        #region Public
        public bool IsInteractable { get; private set; } = true;

        public bool IsActive
        {
            get => CanvasGroup.interactable;
            set => CanvasGroup.interactable = value;
        }

        public bool DontShowOnInit
        {
            get => dontShowOnInit;
            protected set
            {
                if (dontShowOnInit == value)
                    return;

                dontShowOnInit = value;
            }
        }

        public object Data { get; protected set; }
        public object Result { get; protected set; }
        public string PreviousControlName { get; protected set; }
        #endregion

        #region Private
        private Animator animator_ = null;
        private CanvasGroup canvasGroup_ = null;
        private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();
        #endregion

        #region Protected
        protected Animator Animator
        {
            get
            {
                if (!animator_)
                {
                    animator_ = this.gameObject.GetComponent<Animator>();
                    if (!animator_)
                        animator_ = this.gameObject.AddComponent<Animator>();
                }

                return animator_;
            }
        }

        protected CanvasGroup CanvasGroup
        {
            get
            {
                if (!canvasGroup_)
                {
                    canvasGroup_ = this.gameObject.GetComponent<CanvasGroup>();
                    if (!canvasGroup_)
                        canvasGroup_ = this.gameObject.AddComponent<CanvasGroup>();
                }

                return canvasGroup_;
            }
        }
        #endregion

        #region Unity methods
        private void OnCanvasGroupChanged()
        {
            var groupAllowInteraction = true;
            Transform t = transform;
            while (t != null)
            {
                t.GetComponents(m_CanvasGroupCache);
                bool shouldBreak = false;
                for (var i = 0; i < m_CanvasGroupCache.Count; i++)
                {
                    if (!m_CanvasGroupCache[i].interactable)
                    {
                        groupAllowInteraction = false;
                        shouldBreak = true;
                    }

                    if (m_CanvasGroupCache[i].ignoreParentGroups)
                        shouldBreak = true;
                }

                if (shouldBreak)
                    break;

                t = t.parent;
            }

            if (groupAllowInteraction != IsInteractable)
                IsInteractable = groupAllowInteraction;
        }

        private void OnTransformParentChanged() =>
            OnCanvasGroupChanged();
        #endregion

        #region Public methods
        public sealed override void Show() =>
            StartCoroutine(Showing());

        public sealed override void Hide() =>
            StartCoroutine(Hiding());

        public sealed override void Close() =>
            StartCoroutine(Closing());
        #endregion

        #region Private methods
        private void Initialization(ScreenData screenData)
        {
            if (IsInited)
                return;

            Data = screenData.Data;
            PreviousControlName = screenData.PreviousControlName;

            IsRunning = true;

            OnInit?.Invoke(this);

            IsInited = true;

            if (!DontShowOnInit)
                Show();
        }

        private void Destruction()
        {
            IsRunning = false;

            OnDestruct?.Invoke(this);

            Destroy(gameObject);
        }
        #endregion

        #region Coroutines
        private IEnumerator Showing()
        {
            CanvasGroup.alpha = 1.0f;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;

            yield return ShowingAnimation();

            IsShowed = true;

            OnShow?.Invoke(this);
        }

        protected virtual IEnumerator ShowingAnimation()
        {
            if (Animator && Animator.runtimeAnimatorController != null && !IsShowed)
            {
                Animator.SetBool("showed", true);

                yield return new WaitForEndOfFrame();

                var duration = 0.0f;
                var layerIdx = Animator.GetLayerIndex("Base Layer");
                var clipInfo = Animator.GetCurrentAnimatorClipInfo(layerIdx);
                if (clipInfo != null && clipInfo.Length > 0)
                {
                    var clip = clipInfo[0].clip;
                    if (clip != null)
                        duration = clip.length;
                }
                else
                    Debug.LogWarning("ClipInfo not found!");

                yield return new WaitForSecondsRealtime(duration);
            }
        }

        private IEnumerator Hiding()
        {
            yield return HidingAnimation();

            CanvasGroup.alpha = 0.0f;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;

            IsShowed = false;

            OnHide?.Invoke(this);
        }

        protected virtual IEnumerator HidingAnimation()
        {
            if (Animator && Animator.runtimeAnimatorController != null && IsShowed)
            {
                Animator.SetBool("showed", false);

                yield return new WaitForEndOfFrame();

                var duration = 0.0f;
                var layerIdx = Animator.GetLayerIndex("Base Layer");
                var clipInfo = Animator.GetCurrentAnimatorClipInfo(layerIdx);
                if (clipInfo != null && clipInfo.Length > 0)
                {
                    var clip = clipInfo[0].clip;
                    if (clip != null)
                        duration = clip.length;
                }
                else
                    Debug.LogWarning("ClipInfo not found!");

                yield return new WaitForSecondsRealtime(duration);
            }
        }

        private IEnumerator Closing()
        {
            OnClose?.Invoke(this);

            yield return Hiding();

            Destruction();
        }
        #endregion
    }
}