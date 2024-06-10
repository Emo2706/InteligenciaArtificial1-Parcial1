using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : SteeringAgent
{
    GameManager gm;
    [SerializeField] Hunter _hunter;
    

    [Range(0f, 2f)] public float cohesionWeight = 1;
    [Range(0f, 2f)] public float separationWeight = 1;
    [Range(0f, 2f)] public float alignmentWeight = 1;
    Renderer _renderer;
    [SerializeField] Food _food;


    private void Start()
    {
        gm = GameManager.instance;

        _renderer = GetComponent<Renderer>();

        gm.boids.Add(this);

        var randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        AddForce(randomDir.normalized * _maxSpeed);
    }
    private void Update()
    {
        Move();

        var obsAvoidanceForce = ObstacleAvoidance();

        
        gm.ShiftPositionOnBounds(transform);



        var distFood = (_food.transform.position - transform.position).sqrMagnitude;

        var dist = (_hunter.transform.position - transform.position).sqrMagnitude;

        if (dist <= _viewRadius * _viewRadius && obsAvoidanceForce != Vector3.zero)
        {
            AddForce(Evade(_hunter));
            _renderer.material.color = Color.blue;
        }

        if (distFood <= _viewRadius * _viewRadius)
        {
            _renderer.material.color = Color.yellow;
            AddForce(Arrive(_food.transform.position));
            _food.ChangePos();
        }

        if (dist >= _viewRadius * _viewRadius && distFood >= _viewRadius * _viewRadius)
        {
            Flocking();
            _renderer.material.color = Color.green;
        }


    }

    

   void Flocking()
    {
        AddForce(Cohesion(gm.boids) * cohesionWeight + Separation(gm.boids) * separationWeight + Alignment(gm.boids) * alignmentWeight);
    }

}
