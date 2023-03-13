using FreeTeam.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTeam.BP.UI.Dialogs
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class DialogController : UIController, IUIData, IUIResult
    {
        #region Constants
        private const string SHOWED_ANIMATION_VALUE_NAME = "showed";
        private const string LAYER_ANIMATION_NAME = "Base Layer";
        #endregion

        #region Internal
        public class DialogData
        {
            #region Public
            public object Data { get; private set; }
            #endregion

            public DialogData(object _data)
            {
                Data = _data;
            }
        }
        #endregion

        #region Public
        public bool IsInteractable { get; private set; } = true;

        public bool IsActive
        {
            get => CanvasGroup.interactable;
            set => CanvasGroup.interactable = value;
        }

        public object Data { get; protected set; }
        public object Result { get; protected set; }
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

        #region Private
        private Animator animator_ = null;
        private CanvasGroup canvasGroup_ = null;

        private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();
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

        private void OnTransformParentChanged() => OnCanvasGroupChanged();
        #endregion

        #region Public methods
        public sealed override void Show() => StartCoroutine(Showing());

        public sealed override void Hide() => StartCoroutine(Hiding());

        public sealed override void Close() => StartCoroutine(Closing());
        #endregion

        #region Private methods
        private void Initialization(DialogData _dialogData)
        {
            if (IsInited)
                return;

            Data = _dialogData.Data;

            IsRunning = true;

            OnInit?.Invoke(this);

            IsInited = true;

            Show();
        }

        private void Destruction()
        {
            IsRunning = false;

            OnDestruct?.Invoke(this);

            GameObject.Destroy(gameObject);
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