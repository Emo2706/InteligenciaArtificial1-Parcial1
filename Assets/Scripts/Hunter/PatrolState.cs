using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class PatrolState : State
{
    
    Transform _transform;
    Transform[] _waypoints;
    float _speed;
    int _indexWayPoint;
    float _minDistWayPoint;
    int _maxWaypoints = 5;
    float _viewRadius;
    LayerMask _obstacleMask;
    Material _material;
    Hunter _hunter;

    public PatrolState(Hunter hunter)
    {
        _hunter = hunter;
        _transform = hunter.transform;
        _waypoints = hunter.wayPoints;
        _speed = hunter.speed;
        _viewRadius = hunter.viewRadius;
        _obstacleMask = hunter.obstacleMask;
        _material = hunter.GetComponent<Renderer>().material;
    }

    public override void OnEnter()
    {
        _material.color = Color.red;

    }

    public override void OnUpdate()
    {
        _hunter.energy -= Time.deltaTime;
        if (_hunter.energy <= 0)
            fsm.ChangeState(HunterStates.Rest);

        Move();

        

        Collider[] boids = Physics.OverlapSphere(_transform.position, _viewRadius, _obstacleMask);

        foreach (var boid in boids)
        {
            if (boid.GetComponent<Boids>()!=null)
            {
                _hunter.boid= boid.GetComponent<Boids>();
                fsm.ChangeState(HunterStates.Chase);
            }
        }
        
        
    }
    
    void Move()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _waypoints[_indexWayPoint].position, _speed * Time.deltaTime);
        
        _hunter.dir = _waypoints[_indexWayPoint].position - _transform.position;

        _transform.right = _hunter.dir;
        

        var dist = (_waypoints[_indexWayPoint].position - _transform.position).sqrMagnitude;

        if (dist <= _minDistWayPoint * _minDistWayPoint)
            _indexWayPoint++;

        if (_indexWayPoint >= _maxWaypoints)
            _indexWayPoint = 0;
    }

   
    public override void OnExit()
    {

    }


}*/
