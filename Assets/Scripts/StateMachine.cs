public class StateMachine
{
    public State currentState { get; private set; }

    public void Initialize(State _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(State _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
