using DG.Tweening;
using System.Collections;

namespace FreeTeam.BP.UI.SplashScreens
{
    public class DefaultSplashScreenController : SplashScreenController
    {
        #region Private methods
        public void OnShowed()
        {
        }
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
