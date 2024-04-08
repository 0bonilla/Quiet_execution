using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMono<T> : MonoBehaviour, IState<T>
{
    Dictionary<T, IState<T>> _transitions;
    protected FSM<T> _fsm;

    public virtual void Sleep()
    {
    }
    public virtual void Enter()
    {
    }
    public virtual void Execute()
    {
    }
    public void LateExecute()
    {
    }

    public void AddTransition(T input, IState<T> state)
    {
        _transitions[input] = state;
    }
    public void RemoveTransition(T input)
    {
        if (_transitions.ContainsKey(input))
            _transitions.Remove(input);
    }
    public IState<T> GetTransition(T input)
    {
        if (_transitions.ContainsKey(input))
        {
            return _transitions[input];
        }
        return null;
    }

    public FSM<T> SetFSM { set { _fsm = value; } }
}
