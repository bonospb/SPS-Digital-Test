using FreeTeam.BP.UI.Dialogs;
using FreeTeam.BP.UI.Screens;
using FreeTeam.BP.UI.SplashScreens;
using UnityEngine;
using Zenject;

namespace FreeTeam.BP.Services.App
{
    public class ApplicationService : MonoBehaviour, IInitializable
    {
        #region Public methods
        public void RestartApp()
        {
            // need implemented
        }
        #endregion

        #region Implementation
        public void Initialize() =>
            StartGame();
        #endregion

        #region Private methods
        private async void StartGame()
        {
            ScreensManager.DestroyAllScreens();
            SplashScreensManager.DestroyAllSplashScreens();
            DialogsManager.DestroyAllDialogs();

            var splashScreen = await SplashScreensManager.ShowSplashScreen(SplashScreenNames.LOAD_GAME_SPLASH_SCREEN_NAME);
            await splashScreen.WaitShowing();

            splashScreen.Close();
            await splashScreen.WaitHiding();
        }
        #endregion
    }
}
