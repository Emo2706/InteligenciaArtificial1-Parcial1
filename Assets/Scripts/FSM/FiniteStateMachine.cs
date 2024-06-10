using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    StateIA1 _actualState;
    Dictionary<HunterStates, StateIA1> _states = new Dictionary<HunterStates, StateIA1>();

    public void ChangeState(HunterStates name)
    {
        if (!_states.ContainsKey(name)) return;
        _actualState?.OnExit();
        _actualState = _states[name];
        _actualState.OnEnter();


    }

    public void AddState(HunterStates name , StateIA1 state)
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
