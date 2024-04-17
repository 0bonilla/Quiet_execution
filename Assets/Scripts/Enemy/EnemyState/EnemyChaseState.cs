using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState<T> : State<T>
{
    ISteering _steering;
    Enemy _model;
    ObstacleAvoidance _obs;
    Transform _target;
    public EnemyChaseState(Enemy model,Transform target, ISteering steering, ObstacleAvoidance obs)
    {
        _steering = steering;
        _model = model;
        _obs = obs;
        _target = target;
    }
    public override void Execute()
    {
        var dir = _obs.GetDir(_steering.GetDir(), false);
        Debug.Log(dir);
        _model.Move(_model.transform.forward);
        _model.LookDir(dir);
    }
}
