using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteering
{
    Transform _entity;
    Transform _target;
    Enemy _model;

    public Seek(Enemy model,Transform entity, Transform target)
    {
        _entity = entity;
        _target = target;
        _model = model;
    }

    public Vector3 GetDir()
    {
        //a: entity;
        //b: _target;
        _target = _model.currentWayPoint;
        return (_target.position - _entity.position).normalized;
    }
}
