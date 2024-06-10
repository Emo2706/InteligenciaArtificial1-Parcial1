using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{

    Transform _transform;
    int _maxForce;
    float _speed;
    float _viewRadius;
    Material _material;
    Hunter _hunter;
    LayerMask _obstacleMask;
    GameManager _gm;

    public ChaseState(Hunter hunter , GameManager gm)
    {
        _hunter = hunter;
        _transform = hunter.transform;
        _maxForce = hunter.maxForce;
        _speed = hunter.speed;
        _viewRadius = hunter.viewRadius;
        _obstacleMask = hunter.obstacleMask;
        _material =  hunter.GetComponent<Renderer>().material;
        _gm = gm;

    }

    public override void OnEnter()
    {
        _material.color = Color.grey;
    }


    public override void OnUpdate()
    {
        
        AddForce(Pursuit(_hunter.boid));

        


        _hunter.energy-= Time.deltaTime;

        if (_hunter.energy <= 0)
        {
            fsm.ChangeState(HunterStates.Rest);
        }

        Move();

        _gm.ShiftPositionOnBounds(_transform);
        
    }
    
    void Move()
    {
        _transform.position += _hunter.dir * _speed * Time.deltaTime;
        _transform.right = _hunter.dir;
    }


    public override void OnExit()
    {


    }

    Vector3 Seek(Vector3 targetPos, float speed)
    {
        Vector3 desired = targetPos - _transform.position;
        desired.Normalize();
        desired *= speed;

        Vector3 steering = desired - _hunter.dir;
        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);
        return steering;
    }

    Vector3 Seek(Vector3 targetPos)
    {
        return Seek(targetPos, _speed);
    }

    void AddForce(Vector3 force)
    {
        _hunter.dir = Vector3.ClampMagnitude(_hunter.dir + force, _speed);
    }

    Vector3 Pursuit(Boids boid)
    {
        Vector3 futurePos = boid.transform.position + boid.dir; 
        return Seek(futurePos);
    }

}
