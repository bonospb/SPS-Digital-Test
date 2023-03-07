namespace FreeTeam.BP.FSM
{
    public abstract class StateMachine : IStateMachine
    {
        #region Public
        public bool IsInited { get; private set; }
        public IState CurrentState { get; private set; }
        #endregion

        public StateMachine() { }
        public StateMachine(IState currentState) =>
            Initialize(currentState);

        #region Public methods
        public void Initialize(IState startingState)
        {
            if (IsInited)
                return;

            ChangeState(startingState);

            IsInited = true;
        }

        public void ChangeState(IState newState)
        {
            if (CurrentState != null && CurrentState.Equals(newState))
                return;

            CurrentState?.OnExit();
            CurrentState = newState;
            CurrentState?.OnEnter();
        }
        #endregion
    }
}
