namespace FreeTeam.BP.FSM
{
    public interface IState
    {
        IStateMachine StateMachine { get; }

        void OnEnter();
        void OnExit();
    }
}
