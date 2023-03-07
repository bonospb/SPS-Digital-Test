using DG.Tweening;
using System;
using System.Collections;

namespace FreeTeam.BP.UI.Dialogs
{
    public class PauseDialogController : DialogController
    {
        #region Public
        public event Action OnResumeBtnClick = null;
        public event Action OnExitBtnClick = null;
        #endregion

        #region Public methods
        public void OnResumeBtnClickHandler() =>
            OnResumeBtnClick?.Invoke();

        public void OnExitBtnClickHandler() =>
            OnExitBtnClick?.Invoke();
        #endregion

        #region Coroutines
        protected override IEnumerator ShowingAnimation()
        {
            var tween = CanvasGroup.DOFade(1f, 0.5f);
            yield return tween.WaitForCompletion();
        }

        protected override IEnumerator HidingAnimation()
        {
            var tween = CanvasGroup.DOFade(0f, 0.5f);
            yield return tween.WaitForCompletion();
        }
        #endregion
    }
}
