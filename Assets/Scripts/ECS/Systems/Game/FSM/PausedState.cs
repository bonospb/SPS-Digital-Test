using FreeTeam.BP.Common;
using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.FSM;
using FreeTeam.BP.UI.Dialogs;
using Leopotam.EcsLite;
using UnityEngine;

namespace FreeTeam.BP.ECS.Systems.FSM
{
    public class PausedState : State
    {
        #region Private
        private EcsWorld world = default;

        private TimeScaleTypes prevTimeScaleType = TimeScaleTypes.Normal;

        private PauseDialogController pauseDialogController = default;
        #endregion

        public PausedState(IStateMachine stateMachine, EcsWorld ecsWorld) : base(stateMachine) =>
            world = ecsWorld;

        #region Public methods
        public override void OnEnter()
        {
            Debug.Log($"Enter to state: {GetType().Name}");

            ref var timeScaleData = ref world.GetUnique<TimeScaleData>();

            prevTimeScaleType = timeScaleData.Value;

            timeScaleData.Value = TimeScaleTypes.Paused;

            ConstructUI();
        }

        public override void OnExit()
        {
            Debug.Log($"Exit from state: {GetType().Name}");

            DestructUI();

            world.GetUnique<TimeScaleData>().Value = prevTimeScaleType;
        }
        #endregion

        #region Private methods
        private async void ConstructUI()
        {
            pauseDialogController = await DialogsManager.CreateDialog<PauseDialogController>(DialogNames.PAUSE_DIALOG_NAME);
            pauseDialogController.OnResumeBtnClick += OnResumeBtnClickHandler;
            pauseDialogController.OnExitBtnClick += OnExitBtnClickHandler;
            await pauseDialogController.WaitShowing();

        }

        private async void DestructUI()
        {
            pauseDialogController.OnResumeBtnClick -= OnResumeBtnClickHandler;
            pauseDialogController.OnExitBtnClick -= OnExitBtnClickHandler;
            pauseDialogController.Close();
            await pauseDialogController.WaitHiding();
        }

        private void OnResumeBtnClickHandler()
        {
            ref var gameStateData = ref world.GetUnique<GameStateData>();
            gameStateData.Value = GameStateTypes.Played;
        }

        private void OnExitBtnClickHandler()
        {
            ref var gameStateData = ref world.GetUnique<GameStateData>();
            gameStateData.Value = GameStateTypes.Stoped;
        }
        #endregion
    }
}
