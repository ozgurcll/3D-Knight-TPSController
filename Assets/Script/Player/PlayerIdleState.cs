using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        if (xInput != 0 || yInput != 0 && !player.isBusy)
            player.stateMachine.ChangeState(player.moveState);

        /* if (Input.GetKeyDown(KeyCode.W) && !player.isBusy)
             player.stateMachine.ChangeState(player.moveState);*/



    }

}
