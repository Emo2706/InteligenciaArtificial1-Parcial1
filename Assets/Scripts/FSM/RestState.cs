using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestState : StateIA1
{
    int _maxEnergy;
    Material _material;
    Hunter _hunter;
    Transform _transform;
    float _viewRadius;
    LayerMask _obstacleMask;

    public RestState(Hunter h)
    {
        _hunter = h;
        _maxEnergy = h._maxEnergy;
        _viewRadius = h.viewRadius;
        _transform = h.transform;
        _obstacleMask = h.obstacleMask;
        _material = _material = h.GetComponent<Renderer>().material;

    }

    public override void OnEnter()
    {
        _material.color = Color.blue;
    }

    public override void OnUpdate()
    {
        _hunter.energy += Time.deltaTime;

        if (_hunter.energy>=_maxEnergy)
        {
            Collider[] boids = Physics.OverlapSphere(_transform.position, _viewRadius, _obstacleMask);

            foreach (var boid in boids)
            {
                if (boid.GetComponent<Boids>() != null)
                {
                    _hunter.actualBoid = boid.GetComponent<Boids>();
                    fsm.ChangeState(HunterStates.Chase);

                }
            }
            fsm.ChangeState(HunterStates.Patrol);
        }
    }


    public override void OnExit()
    {
   
    }


}
