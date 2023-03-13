using FreeTeam.BP.Configuration;
using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.FSM;
using FreeTeam.BP.UI.SplashScreens;
using Leopotam.EcsLite;
using System.Linq;
using UnityEngine;

namespace FreeTeam.BP.ECS.Systems.FSM
{
    public class InitState : State
    {
        #region Private
        private readonly EcsWorld world = default;
        private readonly Configurations configs = default;

        private LoadGameSplashScreenController loadGameSplashScreenController = default;
        #endregion

        public InitState(IStateMachine stateMachine, EcsWorld ecsWorld, Configurations configurations) : base(stateMachine)
        {
            world = ecsWorld;
            configs = configurations;
        }

        #region Public methods
        public override void OnEnter()
        {
            Debug.Log($"Enter to state: {GetType().Name}");

            ConstructUI();

            SetLevelData();

            world.AddUnique<LoadLevelEvent>();
        }

        public override void OnExit()
        {
            Debug.Log($"Exit from state: {GetType().Name}");

            DestructUI();
        }
        #endregion

        #region Private methods
        private async void ConstructUI()
        {
            loadGameSplashScreenController = await SplashScreensManager.CreateSplashScreen<LoadGameSplashScreenController>(SplashScreenNames.LOAD_GAME_SPLASH_SCREEN_NAME);
            await loadGameSplashScreenController.WaitShowing();
        }

        private async void DestructUI()
        {
            loadGameSplashScreenController.Close();
            await loadGameSplashScreenController.WaitHiding();
        }

        private void SetLevelData()
        {
            ref var profileData = ref world.GetUnique<ProfileData>();
            ref var levelData = ref world.ReplaceUnique<LevelData>();

            var progressData = profileData.Value.ProgressData;

            levelData.LevelId = (progressData.Count <= 1) ? configs.Levels.First().Value.Id : progressData.Last().LevelId;
        }
        #endregion
    }
}
