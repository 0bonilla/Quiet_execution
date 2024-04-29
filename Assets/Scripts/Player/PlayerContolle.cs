using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Player _player;
    IPlayerView _view;

    FSM<StatesEnum> _fsm;
    private void Awake()
    {
        _player = GetComponent<Player>();
        _view = GetComponent<IPlayerView>();
        InitializeFSM();
    }

    void InitializeFSM()
    {
        _fsm = new FSM<StatesEnum>();

        var idle = new PlayerStateIdle<StatesEnum>(StatesEnum.Walk, StatesEnum.Attack);
        var walk = new PlayerStateWalk<StatesEnum>(_player, _view, StatesEnum.Idle, StatesEnum.Attack);
        var attack = new PlayerStateAttack<StatesEnum>(_player, _view, StatesEnum.Idle, StatesEnum.Walk);

        idle.AddTransition(StatesEnum.Walk, walk);
        idle.AddTransition(StatesEnum.Attack, attack);

        attack.AddTransition(StatesEnum.Walk, walk);
        attack.AddTransition(StatesEnum.Idle, idle);

        walk.AddTransition(StatesEnum.Idle, idle);
        walk.AddTransition(StatesEnum.Attack, attack);

        _fsm.SetInit(idle);
    }
    void Update()
    {
        // verifico si el jugador esta muerto y le cancelo los inputs
        if (!_player.dead)
        {
            _fsm.OnUpdate();
            _player.LookDir();
            _player.AttackCooldown += Time.deltaTime;
        }
    }
}
