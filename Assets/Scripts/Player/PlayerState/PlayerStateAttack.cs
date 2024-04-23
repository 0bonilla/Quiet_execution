using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAttack<T> : State<T>
{
    IPlayerModel _player;
    IPlayerView _view;
    T _idleInput;
    T _inputMovement;
    public PlayerStateAttack(IPlayerModel player, IPlayerView view, T idleInput, T inputMovement)
    {
        _player = player;
        _view = view;
        _idleInput = idleInput;
        _inputMovement = inputMovement;
    }
    public override void Execute()
    {
        base.Execute();
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        _player.Attack();
        if (x == 0 || z == 0)
        {
            //Transition
            _fsm.Transition(_idleInput);
        }
        else if (x != 0 || z != 0)
        {
            _fsm.Transition(_inputMovement);
        }
    }
}
