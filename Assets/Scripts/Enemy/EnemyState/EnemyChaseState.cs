using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState<T> : State<T>
{
    Enemy _model;
    Transform _target;

    public EnemyChaseState(Enemy model, Transform target)
    {
        _model = model;
        _target = target;
    }

    public override void Execute()
    {
        base.Execute();

        Vector3 dir = _target.position - _model.transform.position;
        _model.Move(dir.normalized);
        _model.LookDir(dir.normalized);
    }
}
