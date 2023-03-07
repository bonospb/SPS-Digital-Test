using FreeTeam.UI;
using System.Collections;
using UnityEngine;

namespace FreeTeam.BP.UI.SplashScreens
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class SplashScreenController : UIController, IUIData
    {
        #region Internal
        public class SplashScreenData
        {
            public SplashScreenData(object _data)
            {
                Data = _data;
            }

            #region Public
            public object Data { get; private set; }
            #endregion
        }
        #endregion

        #region Public
        public object Data { get; protected set; }
        public bool ShowOnInit { get; set; } = true;
        #endregion

        #region Private
        private Animator animator_;
        private CanvasGroup canvasGroup_;
        #endregion

        #region Properties
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

        #region Public methods
        public sealed override void Show()
        {
            StartCoroutine(Showing());
        }

        public sealed override void Hide()
        {
            StartCoroutine(Hiding());
        }

        public sealed override void Close()
        {
            StartCoroutine(Closing());
        }
        #endregion

        #region Private methods
        private void Initialization(SplashScreenData _splashScreenData)
        {
            if (IsInited)
                return;

            Data = _splashScreenData.Data;

            IsRunning = true;

            OnInit?.Invoke(this);

            IsInited = true;

            if (ShowOnInit)
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
            CanvasGroup.alpha = 0.0f;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;

            yield return ShowingAnimation();

            CanvasGroup.alpha = 1.0f;

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