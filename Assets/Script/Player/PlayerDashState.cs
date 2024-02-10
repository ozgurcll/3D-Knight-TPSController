using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.trailVFX.SetActive(true);
        stateTimer = player.dashDuration;
    }
    public override void Exit()
    {
        base.Exit();
        player.trailVFX.SetActive(false);
        player.SetVelocity(0, rb.velocity.y);
        player.isDashing = false;
    }
    public override void Update()
    {
        base.Update();

        Ability(player.dashSpeed);

        if (stateTimer < 0 && player.currentWeaponInHand == null)
            player.stateMachine.ChangeState(player.idleState);

        if (stateTimer < 0 && player.currentWeaponInHand != null)
            player.stateMachine.ChangeState(player.battlePoseState);


    }
}
