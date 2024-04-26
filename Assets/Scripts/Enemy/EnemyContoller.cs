using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoller : MonoBehaviour
{
    public Rigidbody target;

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
    }

    void InitializeFSM()
    {
        var idle = new EnemyIdleState<StatesEnum>();
        var dead = new EnemyDeadState<StatesEnum>(_model);
        var attack = new EnemyAttackState<StatesEnum>(_model);
        var chase = new EnemyChaseState<StatesEnum>(_model, target.transform, _pursuit, _obstacleAvoidance);
        var patrol = new EnemyPatrolState<StatesEnum>(_model, _steering, _obstacleAvoidance);

        idle.AddTransition(StatesEnum.Dead, dead);
        idle.AddTransition(StatesEnum.Attack, attack);
        idle.AddTransition(StatesEnum.Chase, chase);
        idle.AddTransition(StatesEnum.Patrol, patrol);

        attack.AddTransition(StatesEnum.Idle, idle);
        attack.AddTransition(StatesEnum.Dead, dead);
        attack.AddTransition(StatesEnum.Chase, chase);

        chase.AddTransition(StatesEnum.Idle, idle);
        chase.AddTransition(StatesEnum.Dead, dead);
        chase.AddTransition(StatesEnum.Attack, attack);

        _fsm = new FSM<StatesEnum>(idle);
    }
    void InitializeSteerings()
    {
        var seek = new Seek(_model,_model.transform, _model.currentWaypoint);
        var pursuit = new Pursuit(_model.transform, target, timePrediction);
        _steering = seek;
        _pursuit = pursuit;
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
        var qKeepChaising = new QuestionNode(QuestionChaseTime, chase, idle);
        var qLoS = new QuestionNode(QuestionLoS, qAttackRange, qPatrol);
        var qHasLife = new QuestionNode(() => _model.Life > 0, qLoS, dead);

        _root = qHasLife;
    }
    bool QuestionChaseTime()
    {
        StartCoroutine(ChaseTime());
        return chasing;
    }
    IEnumerator ChaseTime()
    {
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
        var currLoS = _los.CheckRange(target.transform)
                    && _los.CheckAngle(target.transform)
                    && _los.CheckView(target.transform);
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
        return true;
    }
    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }
}
