using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EnemyContoller : MonoBehaviour
{
    public Transform target;

    #region Stats
    public float attackRange;
    bool seen = false;
    bool chasing;
    Coroutine coroutine;
    #endregion

    #region Steering
    public float timePrediction;
    public float angle;
    public float radius;
    public float personalArea;
    public LayerMask maskObs;
    #endregion

    ILineOfSight _los;
    ISteering _steering;
    ISteering _pursuit;
    FSM<StatesEnum> _fsm;
    Enemy _model;
    ITreeNode _root;
    ObstacleAvoidance _obstacleAvoidance;

    EnemyPatrolState<StatesEnum> _stateFollowPoints;
    [SerializeField] AgentController _agentController;

    private void Awake()
    {
        _model = GetComponent<Enemy>();
        _los = GetComponent<ILineOfSight>();
    }

    private void Start()
    {
        InitializeSteerings();
        InitializedTree();
        InitializeFSM();
        if (_agentController != null)
        {
            IncreaseWaypontIndex();
            _agentController.RunAStar();
        }        
    }

    void InitializeFSM()
    {
        var idle = new EnemyIdleState<StatesEnum>();
        var dead = new EnemyDeadState<StatesEnum>(_model);
        var attack = new EnemyAttackState<StatesEnum>(_model);
        var chase = new EnemyChaseState<StatesEnum>(_model, target.transform, _steering, _obstacleAvoidance);
        _stateFollowPoints = new EnemyPatrolState<StatesEnum>(_model);

        idle.AddTransition(StatesEnum.Dead, dead);
        idle.AddTransition(StatesEnum.Attack, attack);
        idle.AddTransition(StatesEnum.Chase, chase);
        idle.AddTransition(StatesEnum.Patrol, _stateFollowPoints);

        attack.AddTransition(StatesEnum.Idle, idle);
        attack.AddTransition(StatesEnum.Dead, dead);
        attack.AddTransition(StatesEnum.Chase, chase);
        attack.AddTransition(StatesEnum.Patrol, _stateFollowPoints);

        chase.AddTransition(StatesEnum.Idle, idle);
        chase.AddTransition(StatesEnum.Dead, dead);
        chase.AddTransition(StatesEnum.Attack, attack);
        chase.AddTransition(StatesEnum.Patrol, _stateFollowPoints);

        _stateFollowPoints.AddTransition(StatesEnum.Idle, idle);
        _stateFollowPoints.AddTransition(StatesEnum.Dead, dead);
        _stateFollowPoints.AddTransition(StatesEnum.Attack, attack);
        _stateFollowPoints.AddTransition(StatesEnum.Chase, chase);


        _fsm = new FSM<StatesEnum>(idle);
    }
    void InitializeSteerings()
    {
        //Utilizamos seek para el patruyaje y pursuit para perseguir al jugador
        //var seek = new Seek(_model,_model.transform, _model.currentWayPoint);
        //var pursuit = new Pursuit(_model.transform, target, timePrediction);
        //_steering = seek;
        //_pursuit = pursuit;
        _steering = GetComponent<FlockingManager>();
        _obstacleAvoidance = new ObstacleAvoidance(_model.transform, angle, radius, personalArea, maskObs);
    }
    void InitializedTree()
    {
        //Actions
        var idle = new ActionNode(() => _fsm.Transition(StatesEnum.Idle));
        var dead = new ActionNode(() => _fsm.Transition(StatesEnum.Dead));
        var attack = new ActionNode(() => _fsm.Transition(StatesEnum.Attack));
        var chase = new ActionNode(() => _fsm.Transition(StatesEnum.Chase));
        var patrol = new ActionNode(() => _fsm.Transition(StatesEnum.Patrol));

        //Questions
        var qAttackRange = new QuestionNode(QuestionAttackRange, attack, chase);
        var qPatrol = new QuestionNode(QuestionPatrol, patrol, idle);
        var qLoS = new QuestionNode(QuestionLoS, qAttackRange, qPatrol);
        var qHasLife = new QuestionNode(() => _model.Life > 0, qLoS, dead);

        _root = qHasLife;
    }
    IEnumerator ChaseTime()
    {
        // Seguir al jugador segundos despues de dejar de verlo
        Debug.Log("Sigo");
        yield return new WaitForSeconds(_model._totalChaseTime);
        Debug.Log("dejo de seguir");
        seen = false;
    }
    bool QuestionAttackRange()
    {
        return _los.CheckRange(target.transform, attackRange);
    }
    bool QuestionLoS()
    {
        // verificamos si el enemigo nos puede ver.
        var currLoS = _los.CheckRange(target.transform)
                    && _los.CheckAngle(target.transform)
                    && _los.CheckView(target.transform);

        /* Utilizamos corrutinas para poder lograr que el enemigo te persiga   
            despues de cierto tiempo */
        if (currLoS == false && seen == true)
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(ChaseTime());
            }
            return true;
        }
        if (coroutine != null)
        {
            StopCoroutine(ChaseTime());
            coroutine = null;
        }
        seen = currLoS;
        return seen;
    }
    bool QuestionPatrol()
    {
        // preguntamos si tiene patrullaje
        if (_model.waypoints.Length == 0)
        {
            return false;
        }
        else return true;
    }
    private void IncreaseWaypontIndex()
    {
        //Se randomiza el currentWaypointIndex de manera controlada
        _model.currentWaypointIndex = MyRandoms.RangeRandom(0, _model.waypoints.Length);
        if (_model.index == _model.currentWaypointIndex)
        {
            _model.currentWaypointIndex++;
        }
        if (_model.currentWaypointIndex >= _model.waypoints.Length)
        {
            _model.currentWaypointIndex = 0;
        }
        _model.currentWayPoint = _model.waypoints[_model.currentWaypointIndex];
        _agentController.target = _model.currentWayPoint;
        _agentController.RunAStar();
    }
    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
            _model.index = _model.currentWaypointIndex;
            IncreaseWaypontIndex();
        }
    }
    public IPoints GetStateWaypoints => _stateFollowPoints;
}
