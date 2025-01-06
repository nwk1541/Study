using System.Collections;
using System.Collections.Generic;

public enum States
{
    Idle,
    Seek,
    Move,
    Attack
}

public class StateController
{
    private StateBase _currentState;

    public StateController(StateBase state)
    {
        _currentState = state;
    }

    public void ForwardState()
    {
        // TODO : 
    }

    public void UpdateState()
    {
        // TODO : 
    }
}
