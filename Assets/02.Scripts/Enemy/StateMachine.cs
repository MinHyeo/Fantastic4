using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum StateType 
{
    None,
    Idle,
    Walk,
    Dead,
}

public class StateMachine
{
    private StateType _currentState;
    private Dictionary<StateType, IState> _stateList = new Dictionary<StateType, IState>();

    public void AddState(StateType stateType, IState state)
    {
        if (_stateList.ContainsKey(stateType) == true)
            return;

        _stateList.Add(stateType, state);
    }

    public void ChangeState(StateType state)
    {
        if (_currentState == state)
            return;
        if (_stateList.ContainsKey(state) == false)
            return;

        _stateList[_currentState].ExitState();
        _currentState = state;
        _stateList[_currentState].EnterState();
    }

    public void UpdateState()
    {
        _stateList[_currentState].UpdateState();
    }
}
