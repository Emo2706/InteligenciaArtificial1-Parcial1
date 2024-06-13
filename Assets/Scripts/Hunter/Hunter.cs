using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Hunter : MonoBehaviour
{
    GameManager _gm;
    public float speed;
    public float energy;
    public int _maxEnergy = 30;
    public float viewRadius;
    public Transform[] wayPoints;
    public LayerMask obstacleMask = 1<<8;
    public int maxForce;
    int _indexWayPoint;
    float _minDistWayPoint = 0.02f;
    [SerializeField] float _minDist = 0.02f;
    [SerializeField] List<SteeringAgent> boidsList = new List<SteeringAgent>();

    EventFSM<HunterStates> _stateMachine;
    
    public Color actualColor;
    public Boids actualBoid;
    public Vector3 dir;
    Material _material;
    Action MovePatrol;
    Action MoveChase;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;

        #region StateCreator
        var patrol = new State<HunterStates>("Patrol");
        var chase = new State<HunterStates>("Chase");
        var rest = new State<HunterStates>("Rest");
        #endregion

        #region Transitions
        StateConfigurer.Create(patrol)
                        .SetTransition(HunterStates.Chase, chase)
                        .SetTransition(HunterStates.Rest, rest)
                        .Done();


        StateConfigurer.Create(chase)
                        .SetTransition(HunterStates.Patrol, patrol)
                        .SetTransition(HunterStates.Rest, rest)
                        .Done();

        StateConfigurer.Create(rest)
                        .SetTransition(HunterStates.Patrol, patrol)
                        .SetTransition(HunterStates.Chase, chase)
                        .Done();
        #endregion

        #region Patrol

        patrol.OnEnter += x =>
         {
             _material.color = Color.magenta;
         };

        patrol.OnUpdate += () =>
        {
            energy -= Time.deltaTime;

            if (energy <= 0)
                ChangeState(HunterStates.Rest);


            

            actualBoid = boids(boidsList).FirstOrDefault();

            if (actualBoid != null) ChangeState(HunterStates.Chase);

            /*Collider[] boids = Physics.OverlapSphere(transform.position, viewRadius, obstacleMask);

            foreach (var boid in boids)
            {
                if (boid.GetComponent<Boids>() != null)
                {
                    actualBoid = boid.GetComponent<Boids>();
                    ChangeState(HunterStates.Chase);
                }
            }*/
        };

        MovePatrol += () =>
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[_indexWayPoint].position, speed * Time.deltaTime);

            dir = wayPoints[_indexWayPoint].position - transform.position;

            transform.right = dir;


            var dist = (wayPoints[_indexWayPoint].position - transform.position).sqrMagnitude;

            if (dist <= _minDistWayPoint * _minDistWayPoint)
                _indexWayPoint++;

            if (_indexWayPoint >= wayPoints.Length)
                _indexWayPoint = 0;
        };


        patrol.OnFixedUpdate += MovePatrol;
         
        #endregion

        #region Chase
        chase.OnEnter += x =>
         {
             _material.color = Color.green;
         };

        chase.OnUpdate += () =>
        {
            AddForce(Pursuit(actualBoid));

            energy -= Time.deltaTime;

            if (energy <= 0) ChangeState(HunterStates.Rest);

            var dist = actualBoid.transform.position - transform.position;

            if (dist.sqrMagnitude <= _minDist*_minDist)
            {
                
                ChangeState(HunterStates.Patrol);
            }
            _gm.ShiftPositionOnBounds(transform);
        };

        MoveChase += () =>
        {
            transform.position += dir * speed * Time.deltaTime;
            transform.right = dir;
        };

        chase.OnFixedUpdate += MoveChase;
        

        #endregion

        #region Rest

        rest.OnEnter += x =>
        {
            _material.color = Color.blue;
        };

        rest.OnUpdate += () =>
        {
            energy += Time.deltaTime;

            if (energy >= _maxEnergy)
            {
                Collider[] boids = Physics.OverlapSphere(transform.position, viewRadius, obstacleMask);

                foreach (var boid in boids)
                {
                    if (boid.GetComponent<Boids>() != null)
                    {
                        actualBoid = boid.GetComponent<Boids>();
                       ChangeState(HunterStates.Chase);

                    }
                }
                ChangeState(HunterStates.Patrol);
            }
        };
        #endregion

        _stateMachine = new EventFSM<HunterStates>(patrol);
    }

    void ChangeState(HunterStates state) => _stateMachine.SendInput(state);
    // Start is called before the first frame update
    void Start()
    {
        energy = _maxEnergy;
        _gm = GameManager.instance;
        
 
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Update();
        
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    Vector3 Seek(Vector3 targetPos, float speed)
    {
        Vector3 desired = targetPos - transform.position;
        desired.Normalize();
        desired *= speed;

        Vector3 steering = desired - dir;
        steering = Vector3.ClampMagnitude(steering, maxForce * Time.deltaTime);
        return steering;
    }

    Vector3 Seek(Vector3 targetPos)
    {
        return Seek(targetPos, speed);
    }

    void AddForce(Vector3 force)
    {
        dir = Vector3.ClampMagnitude(dir + force, speed);
    }

    Vector3 Pursuit(Boids boid)
    {
        Vector3 futurePos = boid.transform.position + boid.dir;
        return Seek(futurePos);
    }

    IEnumerable<Boids> boids(IEnumerable<SteeringAgent> boidsList)
    {
        var list = boidsList.Where(x => (x.transform.position - transform.position).sqrMagnitude <= viewRadius * viewRadius)
                            .OrderBy(x => (x.transform.position - transform.position).sqrMagnitude <= viewRadius * viewRadius)
                            .OfType<Boids>();

        Debug.Log(list);
        return list;
    }
}

public enum HunterStates
{
    Rest,
    Patrol,
    Chase
}