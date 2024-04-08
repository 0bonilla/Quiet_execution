using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDeadState<T> : State<T>
{
    Enemy _model;

    public EnemyDeadState(Enemy model)
    {
        _model = model;
    }

    public override void Enter()
    {
        base.Enter();
        _model.Dead();
    }
}
