using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringAgent : MonoBehaviour
{
    public Vector3 dir;
    [SerializeField] protected float _maxSpeed = 5;
    [SerializeField] protected float _maxForce = 1;
    [SerializeField] protected float _viewRadius;
    [SerializeField] protected float _viewRadiusSeparation;
    [SerializeField] protected LayerMask obstacleMask = 1<<6 ;
    float _lenght = 0.5f;



    

    protected Vector3 Seek(Vector3 targetPos, float speed)
    {
        Vector3 desired = targetPos - transform.position;
        desired.Normalize();
        desired *= speed;

        Vector3 steering = desired - dir;
        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);
        return steering;
    }


    protected Vector3 Seek(Vector3 seekTarget)
    {
        return Seek(seekTarget, _maxSpeed);
    }

    protected Vector3 Arrive(Vector3 targetPos)
    {
        float dist = (targetPos - transform.position).sqrMagnitude;

        if (dist > _viewRadius * _viewRadius) return Seek(targetPos);

        return Seek(targetPos, (_maxSpeed * (dist / _viewRadius)));
    }

    protected Vector3 Flee(Vector3 targetPos)
    {
        return -Seek(targetPos);
    }

    protected Vector3 ObstacleAvoidance()
    {

        if (Physics.Raycast(transform.position + transform.up * _lenght, transform.right, _viewRadius, obstacleMask))
        {
            Vector3 desired = -transform.up * _maxSpeed;

            return CalculateSteering(desired);
        }

        else if (Physics.Raycast(transform.position, transform.right, _viewRadius, obstacleMask))
        {
            Vector3 desired = transform.up * _maxSpeed;

            return CalculateSteering(desired);
        }

        else if (Physics.Raycast(transform.position - transform.up * _lenght, transform.right, _viewRadius, obstacleMask))
        {
            Vector3 desired = transform.up * _maxSpeed;

            return CalculateSteering(desired);
        }

        else return default;
    }

    protected void AddForce(Vector3 force)
    {
        dir = Vector3.ClampMagnitude(dir + force, _maxSpeed);
    }

    Vector3 CalculateSteering(Vector3 desired)
    {
        Vector3 steering = desired - dir;

        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);
        return steering;
    }

    #region Flocking

    protected Vector3 Cohesion (List<SteeringAgent> agents)
    {
        Vector3 desired = Vector3.zero;
        int count = 0;
        foreach (var item in agents)
        {
            if (item == this) continue;
            if ((item.transform.position - transform.position).sqrMagnitude > _viewRadius * _viewRadius) continue;

            desired += item.transform.position;
            count++;
        }

        if (count == 0) return Vector3.zero;

        desired /= count;
        return Arrive(desired);
    }

    protected Vector3 Separation(List<SteeringAgent> agents)
    {
        Vector3 desired = Vector3.zero;

        foreach (var item in agents)
        {
            if (item == this) continue;
            Vector3 dist = item.transform.position - transform.position;

            if (dist.sqrMagnitude > _viewRadiusSeparation * _viewRadiusSeparation) continue;

            desired += dist;
        }

        if (desired == Vector3.zero) return desired;

        desired *= -1;

        return CalculateSteering(desired.normalized * _maxSpeed);
    }

    protected Vector3 Alignment(List<SteeringAgent> agents)
    {
        Vector3 desired = Vector3.zero;
        int count = 0;

        foreach (var item in agents)
        {
            if (item == this) continue;
            if ((item.transform.position - transform.position).sqrMagnitude > _viewRadius * _viewRadius) continue;

            desired += item.dir;
            count++;
        }

        if (count == 0) return Vector3.zero;

        desired /= count;

        return CalculateSteering(desired.normalized * _maxSpeed);
    }

    #endregion

    protected Vector3 Evade(Hunter hunter)
    {
        Vector3 futurePos = hunter.transform.position + hunter.dir;

        return Flee(futurePos);
    }

    protected void Move()
    {
        transform.position += dir * _maxSpeed * Time.deltaTime;

        transform.right = dir;
    }

    

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);
        
        Vector3 originA = transform.position + transform.up * _lenght;
        Vector3 originB = transform.position - transform.up * _lenght;

        Gizmos.DrawLine(originA,originA + transform.right * _viewRadius);
        Gizmos.DrawLine(originB,originB + transform.right * _viewRadius);
    }

   
}
