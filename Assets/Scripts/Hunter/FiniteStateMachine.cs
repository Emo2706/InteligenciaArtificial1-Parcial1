using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    State _actualState;
    Dictionary<HunterStates, State> _states = new Dictionary<HunterStates, State>();

    public void ChangeState(HunterStates name)
    {
        if (!_states.ContainsKey(name)) return;
        _actualState?.OnExit();
        _actualState = _states[name];
        _actualState.OnEnter();


    }

    public void AddState(HunterStates name , State state)
    {
        if (!_states.ContainsKey(name))
            _states.Add(name, state);
        else
            _states[name] = state;

        state.fsm = this;
    }

    public void ArtificialUpdate()
    {
        _actualState?.OnUpdate();
    }


}
