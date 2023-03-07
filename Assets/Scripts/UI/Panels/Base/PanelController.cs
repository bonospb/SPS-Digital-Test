using FreeTeam.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTeam.BP.UI.Panels
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelController : UIController
    {
        #region Constants
        private const string SHOWED_ANIMATION_VALUE_NAME = "showed";
        private const string LAYER_ANIMATION_NAME = "Base Layer";
        #endregion

        #region Public
        public bool IsInteractable { get; private set; } = true;

        public bool IsActive
        {
            get => CanvasGroup.interactable;
            set => CanvasGroup.interactable = value;
        }
        #endregion

        #region Protected
        protected Animator Animator
        {
            get
            {
                if (!animator_)
                    animator_ = this.gameObject.GetComponent<Animator>();

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

        protected bool dontShowOnInit = false;
        #endregion

        #region Private
        private Animator animator_ = null;
        private CanvasGroup canvasGroup_ = null;

        private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();
        #endregion

        #region Unity methods
        private void Start() => Initialization();
        private void OnDestroy() => Destruction();

        private void OnCanvasGroupChanged()
        {
            // Figure out if parent groups allow interaction
            // If no interaction is alowed... then we need
            // to not do that :)
            var groupAllowInteraction = true;
            Transform t = transform;
            while (t != null)
            {
                t.GetComponents(m_CanvasGroupCache);
                bool shouldBreak = false;
                for (var i = 0; i < m_CanvasGroupCache.Count; i++)
                {
                    // if the parent group does not allow interaction
                    // we need to break
                    if (!m_CanvasGroupCache[i].interactable)
                    {
                        groupAllowInteraction = false;
                        shouldBreak = true;
                    }
                    // if this is a 'fresh' group, then break
                    // as we should not consider parents
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

        private void OnTransformParentChanged() => OnCanvasGroupChanged();
        #endregion

        #region Public methods
        public override void Show() => StartCoroutine(Showing());

        public override void Hide() => StartCoroutine(Hiding());

        public override void Close() => StartCoroutine(Closing());
        #endregion

        #region Private methods
        private void Initialization()
        {
            if (IsInited)
                return;

            IsRunning = true;

            OnInit?.Invoke(this);

            IsInited = true;

            if (!dontShowOnInit)
                Show();
        }

        private void Destruction()
        {
            IsRunning = false;

            OnDestruct?.Invoke(this);
        }
        #endregion

        #region Coroutines
        private IEnumerator Showing()
        {
            if (!IsShowed)
                yield return ShowingAnimation();

            CanvasGroup.alpha = 1.0f;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;

            IsShowed = true;

            OnShow?.Invoke(this);
        }

        protected virtual IEnumerator ShowingAnimation()
        {
            if (Animator && Animator.runtimeAnimatorController != null && !IsShowed)
            {
                Animator.SetBool(SHOWED_ANIMATION_VALUE_NAME, true);

                yield return new WaitForEndOfFrame();

                var duration = 0.0f;
                var layerIdx = Animator.GetLayerIndex(LAYER_ANIMATION_NAME);
                var clipInfo = Animator.GetCurrentAnimatorClipInfo(layerIdx);
                if (clipInfo != null && clipInfo.Length > 0)
                {
                    var clip = clipInfo[0].clip;
                    if (clip != null)
                        duration = clip.length;
                }

                yield return new WaitForSecondsRealtime(duration);
            }
        }

        private IEnumerator Hiding()
        {
            if (IsShowed)
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
                Animator.SetBool(SHOWED_ANIMATION_VALUE_NAME, false);

                yield return new WaitForEndOfFrame();

                var duration = 0.0f;
                var layerIdx = Animator.GetLayerIndex(LAYER_ANIMATION_NAME);
                var clipInfo = Animator.GetCurrentAnimatorClipInfo(layerIdx);
                if (clipInfo != null && clipInfo.Length > 0)
                {
                    var clip = clipInfo[0].clip;
                    if (clip != null)
                        duration = clip.length;
                }

                yield return new WaitForSecondsRealtime(duration);
            }
        }

        private IEnumerator Closing()
        {
            OnClose?.Invoke(this);

            yield return Hiding();

            Destroy(gameObject);
        }
        #endregion
    }
}
