using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerState
{
    public PlayerLandState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (player.isGrounded)
        {
            playerStateMachine.ChangeState(player.idleState);
        }
    }
}
