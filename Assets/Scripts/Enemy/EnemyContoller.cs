using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoller : MonoBehaviour
{
    public Rigidbody target;
    public float attackRange;

    #region Steering
    public float timePrediction;
    public float angle;
    public float radius;
    public LayerMask maskObs;
    #endregion

    ILineOfSight _los;
    ISteering _steering;
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
        var chase = new EnemyChaseState<StatesEnum>(_model, _steering, _obstacleAvoidance);

        idle.AddTransition(StatesEnum.Dead, dead);
        idle.AddTransition(StatesEnum.Attack, attack);
        idle.AddTransition(StatesEnum.Chase, chase);

        dead.AddTransition(StatesEnum.Idle, idle);
        dead.AddTransition(StatesEnum.Attack, attack);
        dead.AddTransition(StatesEnum.Chase, chase);

        attack.AddTransition(StatesEnum.Idle, idle);
        attack.AddTransition(StatesEnum.Dead, dead);
        attack.AddTransition(StatesEnum.Chase, chase);

        chase.AddTransition(StatesEnum.Idle, idle);
        chase.AddTransition(StatesEnum.Dead, dead);
        chase.AddTransition(StatesEnum.Attack, attack);

        _fsm = new FSM<StatesEnum>(chase);
    }
    void InitializeSteerings()
    {
        var seek = new Seek(_model.transform, target.transform);
        _steering = seek;
        _obstacleAvoidance = new ObstacleAvoidance(_model.transform, angle, radius, maskObs);
    }

    void InitializedTree()
    {
        //Actions
        var idle = new ActionNode(() => _fsm.Transition(StatesEnum.Idle));
        var dead = new ActionNode(() => _fsm.Transition(StatesEnum.Dead));
        var attack = new ActionNode(() => _fsm.Transition(StatesEnum.Attack));
        var chase = new ActionNode(() => _fsm.Transition(StatesEnum.Chase));

        //Questions
        var qIsCooldown = new QuestionNode(() => _model.ACooldown, idle, attack);
        var qAttackRange = new QuestionNode(QuestionAttackRange, qIsCooldown, chase);
        var qLoS = new QuestionNode(QuestionLoS, qAttackRange, idle);
        var qHasLife = new QuestionNode(() => _model.Life > 0, qLoS, dead);

        _root = qHasLife;
    }
    bool QuestionAttackRange()
    {
        return _los.CheckRange(target.transform, attackRange);
    }
    bool QuestionLoS()
    {
        return _los.CheckRange(target.transform)
            && _los.CheckAngle(target.transform)
            && _los.CheckView(target.transform);
    }
    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }
}
