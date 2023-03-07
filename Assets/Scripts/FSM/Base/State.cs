namespace FreeTeam.BP.FSM
{
    public abstract class State : IState
    {
        #region Public
        public IStateMachine StateMachine { get; private set; }
        #endregion

        public State(IStateMachine stateMachine) =>
            StateMachine = stateMachine;

        #region Public methods
        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }
        #endregion
    }
}
