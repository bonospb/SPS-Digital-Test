using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.FSM;
using FreeTeam.BP.UI.Screens;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace FreeTeam.BP.ECS.Systems.FSM
{
    public class StopedState : State
    {
        #region Private
        private EcsWorld world = default;

        private MainScreenController mainScreenController = null;
        #endregion

        public StopedState(IStateMachine stateMachine, EcsWorld ecsWorld) : base(stateMachine) =>
            world = ecsWorld;

        #region Public methods
        public override void OnEnter()
        {
            Debug.Log($"Enter to state: {GetType().Name}");

            var fixedUpdateEventEntity = world.NewEntity();
            ref var fixedUpdateGroupEvent = ref world.GetPool<EcsGroupSystemState>().Add(fixedUpdateEventEntity);
            fixedUpdateGroupEvent.Name = "PlaybackFixedUpdateGroup";
            fixedUpdateGroupEvent.State = false;

            var updateEventEntity = world.NewEntity();
            ref var updateGroupEvent = ref world.GetPool<EcsGroupSystemState>().Add(updateEventEntity);
            updateGroupEvent.Name = "PlaybackUpdateGroup";
            updateGroupEvent.State = false;

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
            mainScreenController = await ScreensManager.CreateScreen<MainScreenController>(ScreenNames.MAIN_SCREEN_NAME);
            mainScreenController.OnPlayBtnClick += OnPlayBtnClickHandler;
            await mainScreenController.WaitShowing();
        }

        private async void DestructUI()
        {
            mainScreenController.OnPlayBtnClick -= OnPlayBtnClickHandler;
            mainScreenController.Close();
            await mainScreenController.WaitHiding();
        }

        private void OnPlayBtnClickHandler() =>
            world.GetUnique<GameStateData>().Value = Common.GameStateTypes.Played;
        #endregion
    }
}
