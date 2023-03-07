using DG.Tweening;
using System;
using System.Collections;

namespace FreeTeam.BP.UI.Screens
{
    public class MainScreenController : ScreenController
    {
        #region Public
        public event Action OnPlayBtnClick = null;
        #endregion

        #region Public methods
        public void OnPlayBtnClickHandler() =>
            OnPlayBtnClick?.Invoke();

        public void OnGarageBtnClickHandler()
        {

        }

        public void OnRemoveAdsBtnClickHandler()
        {

        }

        public void OnSettingsBtnClickHandler()
        {

        }
        #endregion

        #region Coroutines
        protected override IEnumerator ShowingAnimation()
        {
            var tween = CanvasGroup.DOFade(1f, 1.5f);
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
