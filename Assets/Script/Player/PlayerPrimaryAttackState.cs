using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.StartDealDamage();


        if (player.comboCounter > 2 || Time.time - player.lastTimeAttacked > player.comboWindow)
            player.comboCounter = 0;

        player.anim.SetInteger("ComboCounter", player.comboCounter);


        stateTimer = .1f;

    }
    public override void Exit()
    {
        base.Exit();

        player.EndDealDamage();


        player.StartCoroutine("BusyFor", .15f);
        player.comboCounter++;
        player.lastTimeAttacked = Time.time;
        player.isAttacking = false;
    }
    public override void Update()
    {
        base.Update();

        player.CanAttack();

        if (stateTimer < 0)
            player.SetZeroVelocity();


        if (triggerCalled)
        {
            if (player.currentWeaponInHand != null)
                player.stateMachine.ChangeState(player.battlePoseState);
            else
                player.stateMachine.ChangeState(player.idleState);
        }

    }
}
