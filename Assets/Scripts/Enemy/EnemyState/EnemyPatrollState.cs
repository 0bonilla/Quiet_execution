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
        Debug.Log(_model.currentWaypoint);
        var dir = _obs.GetDir(_steering.GetDir(), false);
        _model.Move(_model.transform.forward);
        _model.LookDir(dir);
    }

    //public override void Execute()
    //{
    //    base.Execute();
    //    // Check if the agent has reached the current waypoint
    //    if (_model.isWaiting == false && Vector3.Distance(_model.transform.position, _model.waypoints[_model.currentWaypointIndex].position) < 1.5)
    //    {
    //        Debug.Log("alkjsdlaksjdkasjdasd");
    //        _model.waiTimer += Time.deltaTime;

    //        if (_model.waiTimer >= _model.waitDuration)
    //        {
    //            // Start waiting
    //            _model.isWaiting = true;
    //            ContinuePatrol();
    //            _model.waiTimer = 0;
    //        }
    //    }
    //    else
    //    {
    //        MoveToWaypoint(_model.currentWaypointIndex);
    //    }
    //}
    //private void ContinuePatrol()
    //{
    //    _model.isWaiting = false; // End the waiting period
    //    _steering = new Seek(_model.transform, _model.waypoints[_model.currentWaypointIndex].transform);

    //    // Move to the next waypoint
    //    _model.currentWaypointIndex = (_model.currentWaypointIndex + 1) % _model.waypoints.Length;
    //    MoveToWaypoint(_model.currentWaypointIndex);

    //    // Reverse patrol path if the last waypoint is reached
    //    if (_model.currentWaypointIndex == 0)
    //    {
    //        System.Array.Reverse(_model.waypoints);
    //    }
    //}
    //public void MoveToWaypoint(int index)
    //{
    //    Debug.Log("mover");
    //    // Set the destination to the position of the current waypoint
    //    var dir = _obs.GetDir(_steering.GetDir(), false);
    //    _model.Move(_model.transform.forward);
    //    _model.LookDir(dir);
    //}
}
