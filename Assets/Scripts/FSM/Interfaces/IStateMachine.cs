namespace FreeTeam.BP.FSM
{
    public interface IStateMachine
    {
        bool IsInited { get; }
        IState CurrentState { get; }

        void ChangeState(IState newState);
    }
}
