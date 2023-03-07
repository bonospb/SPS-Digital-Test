using FreeTeam.BP.Common;
using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.FSM;
using FreeTeam.BP.UI.Screens;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace FreeTeam.BP.ECS.Systems.FSM
{
    public class PlayedState : State
    {
        #region Private
        private EcsWorld world = default;

        private HUDScreenController hudScreenController = default;
        #endregion

        public PlayedState(IStateMachine stateMachine, EcsWorld ecsWorld) : base(stateMachine) =>
            world = ecsWorld;

        #region Public methods
        public override void OnEnter()
        {
            Debug.Log($"Enter to state: {GetType().Name}");

            var fixedUpdateEventEntity = world.NewEntity();
            ref var fixedUpdateGroupEvent = ref world.GetPool<EcsGroupSystemState>().Add(fixedUpdateEventEntity);
            fixedUpdateGroupEvent.Name = "PlaybackFixedUpdateGroup";
            fixedUpdateGroupEvent.State = true;

            var updateEventEntity = world.NewEntity();
            ref var updateGroupEvent = ref world.GetPool<EcsGroupSystemState>().Add(updateEventEntity);
            updateGroupEvent.Name = "PlaybackUpdateGroup";
            updateGroupEvent.State = true;

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
            hudScreenController = await ScreensManager.CreateScreen<HUDScreenController>(ScreenNames.HUD_SCREEN_NAME);
            hudScreenController.OnPauseBtnClick += OnPauseBtnClickHandler;
            await hudScreenController.WaitShowing();
        }

        private async void DestructUI()
        {
            hudScreenController.OnPauseBtnClick -= OnPauseBtnClickHandler;
            hudScreenController.Close();
            await hudScreenController.WaitHiding();
        }

        private void OnPauseBtnClickHandler()
        {
            ref var gameStateData = ref world.GetUnique<GameStateData>();
            switch (gameStateData.Value)
            {
                case GameStateTypes.Played:
                    gameStateData.Value = GameStateTypes.Paused;
                    break;
                default:
                case GameStateTypes.Stoped:
                case GameStateTypes.Paused:
                    break;
            }
        }
        #endregion
    }
}
