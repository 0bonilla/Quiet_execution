using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState<T> : State<T>
{
    Enemy _model;
    ISteering _steering;
    ObstacleAvoidance _obs;

    public EnemyPatrolState(Enemy model, ISteering steering, ObstacleAvoidance obs)
    {
        _model = model;
        _obs = obs;
        _steering = steering;
    }

    public override void Execute()
    {
        _model.CalculateDirection();
        var dir = _obs.GetDir(_steering.GetDir(), false);
        _model.Move(_model.transform.forward);
        _model.LookDir(dir);
    }
}
