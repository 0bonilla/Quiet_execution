using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    public void Execute();
    public void LateExecute();
    public void Sleep();
    public void Enter();
    public void AddTransition(T input, IState<T> state);
    public void RemoveTransition(T input);
    IState<T> GetTransition(T input);
    FSM<T> SetFSM { set; }
}
