using FreeTeam.BP.Common;
using FreeTeam.BP.Configuration;
using FreeTeam.BP.Data;
using FreeTeam.BP.Editor;
using FreeTeam.BP.Extensions;
using FreeTeam.BP.UI.Dialogs;
using FreeTeam.BP.UI.Screens;
using FreeTeam.BP.UI.SplashScreens;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace FreeTeam.BP.Services.App
{
    public class ApplicationService : MonoBehaviour, IInitializable
    {
        #region SerializeFields
        [Foldout("Save settings", true)]
        [SerializeField, Tooltip("In seconds")] private float saveDelay = 300f;
        [SerializeField] private int saveRequestAmount = 3;
        #endregion

        #region Public
        public Configurations Configurations => _configurations;
        public ProfileData ProfileData => _profileData;
        public LevelData LevelData => _levelData;
        #endregion

        #region Private
        private Configurations _configurations = null;
        private ProfileData _profileData = null;
        private LevelData _levelData = null;

        private float saveTimer = 0f;
        private int saveRequestCounter = 0;
        #endregion

        #region Public methods
        [Inject]
        public void Construct(Configurations config) =>
            _configurations = config;

        public void RestartGame()
        {
            // need implemented
        }
        #endregion

        #region Implementation
        public void Initialize()
        {
            StartGame();
            SaveProfileTask();
        }
        #endregion

        #region Private methods
        private async void StartGame()
        {
            ScreensManager.DestroyAllScreens();
            SplashScreensManager.DestroyAllSplashScreens();
            DialogsManager.DestroyAllDialogs();

            var splashScreen = await SplashScreensManager.ShowSplashScreen(SplashScreenNames.LOAD_GAME_SPLASH_SCREEN_NAME);
            await splashScreen.WaitShowing();

            _profileData = LoadProfileData();
            _levelData = SetLevelData(_profileData.ProgressData);

            splashScreen.Close();
            await splashScreen.WaitHiding();
        }

        private ProfileData LoadProfileData()
        {
            var profilePath = Path.Combine(Application.persistentDataPath, Paths.PROFILE_PATH);

            ProfileData profile = null;

            if (File.Exists(profilePath))
            {
                try
                {
                    var profileJson = File.ReadAllText(profilePath);
                    profile = profileJson.Deserialize<ProfileData>();
                    profile.SetDirtyWithoutNotify(false);

                    Debug.Log($"<b>[{GetType().Name}]</b> | {profile.UID} is loaded.");
                }
                catch (Exception exception)
                {
                    Debug.LogWarning($"<b>[{GetType().Name}]</b> | Load profile failed! {exception.Message}");
                }
            }

            return profile ?? new ProfileData();
        }

        private void SaveProfileData(ProfileData profile)
        {
            var profilePath = Path.Combine(Application.persistentDataPath, Paths.PROFILE_PATH);

            try
            {
                var profileJson = profile.Serialize(true);
                File.WriteAllText(profilePath, profileJson);

                profile.SetDirtyWithoutNotify(false);

                Debug.Log($"<b>[{GetType().Name}]</b> | {profile.UID} is saved.");
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"<b>[{GetType().Name}]</b> | Save profile failed! {exception.Message}");
            }
        }

        private LevelData SetLevelData(ProgressData levelProgresses)
        {
            if (levelProgresses.Count <= 1)
                return new LevelData(_configurations.Levels.First().Value);

            return new LevelData(_configurations.Levels[levelProgresses.Last().LevelId]);
        }

        private void SaveProfileTask()
        {
            var loop1Task = Task.Run(async () =>
            {
                while (true)
                {
                    if (_profileData != null && _profileData.IsDirty)
                    {
                        if (saveTimer >= saveDelay || saveRequestCounter >= saveRequestAmount)
                        {
                            SaveProfileData(_profileData);

                            saveTimer = 0;
                            saveRequestCounter = 0;
                        }
                    }

                    saveTimer += Time.unscaledDeltaTime;

                    await Task.Delay(100);
                }
            });
        }
        #endregion
    }
}
