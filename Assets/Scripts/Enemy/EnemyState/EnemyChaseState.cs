using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState<T> : State<T>
{
    ISteering _steering;
    Enemy _model;
    ObstacleAvoidance _obs;
    public EnemyChaseState(Enemy model, ISteering steering, ObstacleAvoidance obs)
    {
        _steering = steering;
        _model = model;
        _obs = obs;
    }
    public override void Execute()
    {
        var dir = _obs.GetDir(_steering.GetDir()).normalized;
        _model.Move(dir);
        _model.LookDir(dir);
    }
}
