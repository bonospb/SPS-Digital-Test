using FreeTeam.BP.FSM;
using FreeTeam.BP.UI.SplashScreens;
using Leopotam.EcsLite;
using UnityEngine;

namespace FreeTeam.BP.ECS.Systems.FSM
{
    public class InitState : State
    {
        #region Private
        private EcsWorld world = default;

        private DefaultSplashScreenController defaultSplashScreenController = default;
        #endregion

        public InitState(IStateMachine stateMachine, EcsWorld ecsWorld) : base(stateMachine) =>
            world = ecsWorld;

        #region Public methods
        public override void OnEnter()
        {
            Debug.Log($"Enter to state: {GetType().Name}");

            ConstructUI();
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
            defaultSplashScreenController = await SplashScreensManager.CreateSplashScreen<DefaultSplashScreenController>(SplashScreenNames.DEFAULT_SPLASH_SCREEN_NAME);
            await defaultSplashScreenController.WaitShowing();
        }

        private async void DestructUI()
        {
            defaultSplashScreenController.Close();
            await defaultSplashScreenController.WaitHiding();
        }
        #endregion
    }
}
