using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance
{
    float _angle;
    float _radius;
    float _personalArea;
    Transform _entity;
    LayerMask _maskObs;

    public ObstacleAvoidance(Transform entity, float angle, float radius, float personalArea, LayerMask maskObs)
    {
        _angle = angle;
        _radius = radius;
        _entity = entity;
        _maskObs = maskObs;
        _personalArea = personalArea;
    }

    public Vector3 GetDir(Vector3 currentDir, bool calculateY = true)
    {
        Collider[] colls = Physics.OverlapSphere(_entity.position, _radius, _maskObs);
        Collider nearColl = null;
        Vector3 closetPoint = Vector3.zero;
        float nearCollDistance = 0;
        if (!calculateY) currentDir.y = 0;
        for (int i = 0; i < colls.Length; i++)
        {
            var currentColl = colls[i];
            closetPoint = currentColl.ClosestPoint(_entity.position);
            if (!calculateY) closetPoint.y = _entity.position.y;
            Vector3 dirToColl = closetPoint - _entity.position;
            float currentAngle = Vector3.Angle(dirToColl, currentDir);
            float distance = dirToColl.magnitude;

            if (currentAngle > _angle / 2) { continue; }

            if (nearColl == null)
            {
                nearColl = currentColl;
                nearCollDistance = distance;
                continue;
            }

            if (distance < nearCollDistance)
            {
                nearCollDistance = distance;
                nearColl = currentColl;
            }
        }
        if (nearColl == null)
        {
            return currentDir;
        }
        else
        {
            // Determinamos la posición relativa del punto más cercano en el obstáculo.
            Vector3 relativePos = _entity.InverseTransformPoint(closetPoint);
            Vector3 dirToClosestPoint = (closetPoint - _entity.position).normalized;

            // Si el punto más cercano está a la izquierda del objeto, nos desplazamos hacia la derecha y viceversa.
            Vector3 newDir;
            if (relativePos.x < 0)
            {
                newDir = Vector3.Cross(_entity.up, dirToClosestPoint).normalized;
            }
            else
            {
                newDir = -Vector3.Cross(_entity.up, dirToClosestPoint).normalized;
            }

            //Lerp para que sea el cambio de direcciones mas suave. El personalArea servira para que se calcule la distancia, no hacia el punto medio del collider, sino a una distancia minima hacia la entidad.
            return Vector3.Lerp(currentDir, newDir, (_radius - (Mathf.Clamp(nearCollDistance - _personalArea, 0, _radius))) / _radius);
        }
    }
}
