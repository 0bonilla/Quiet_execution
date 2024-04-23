using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle<T> : State<T>
{
    T _inputMovement;
    T _attackMovement;
    public PlayerStateIdle(T inputMovement, T attackMovement)
    {
        _inputMovement = inputMovement;
        _attackMovement = attackMovement;
    }
    public override void Execute()
    {
        base.Execute();
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool Mouse = Input.GetMouseButton(0);

        if (x != 0 || z != 0)
        {
            //Transition
            _fsm.Transition(_inputMovement);
        }
        if (Mouse)
        {
            _fsm.Transition(_attackMovement);
        }
    }
}
