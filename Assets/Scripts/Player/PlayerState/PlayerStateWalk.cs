using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWalk<T> : State<T>
{
    IPlayerModel _player;
    IPlayerView _view;
    T _idleInput;
    T _attackInput;
    public PlayerStateWalk(IPlayerModel player, IPlayerView view, T idleInput, T attackInput)
    {
        _player = player;
        _view = view;
        _idleInput = idleInput;
        _attackInput = attackInput;
    }
    public override void Execute()
    {
        base.Execute();
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool Mouse = Input.GetMouseButton(0);
        Vector3 dir = new Vector3(x, 0, z).normalized;
        _player.Move(dir);

        if (x == 0 && z == 0)
        {
            //Transition
            _fsm.Transition(_idleInput);
        }
        if (Mouse)
        {
            _fsm.Transition(_attackInput);
        }
    }
}

