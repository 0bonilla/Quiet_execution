using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState<T> : State<T>
{
    Enemy _model;

    public EnemyAttackState(Enemy model)
    {
        _model = model;
    }

    public override void Execute()
    {
        base.Execute();
        _model.Attack();
    }
}
