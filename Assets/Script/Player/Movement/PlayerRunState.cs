using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
        player.isRunning = false;
    }
    public override void Update()
    {
        base.Update();
        Movement_Dodge_Dash(3f);

        if (xInput == 0 && yInput == 0)
            player.stateMachine.ChangeState(player.idleState);
        else
            player.stateMachine.ChangeState(player.moveState);
    }
}
