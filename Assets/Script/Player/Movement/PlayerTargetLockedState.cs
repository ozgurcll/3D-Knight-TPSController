using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetLockedState : PlayerState
{
    public PlayerTargetLockedState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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
        player.LookAtTarget();
        Movement_Dodge_Dash(3f);

        if (!player.lockedTarget && player.currentWeaponInHand == null)
            player.stateMachine.ChangeState(player.idleState);
        else if (!player.lockedTarget && player.currentWeaponInHand != null)
            player.stateMachine.ChangeState(player.battlePoseState);

    }
}
