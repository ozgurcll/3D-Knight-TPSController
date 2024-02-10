using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSheathSwordState : PlayerState
{
    public PlayerSheathSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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

        if (player.currentWeaponInHand != null)
            player.stateMachine.ChangeState(player.idleState);
        else
            player.stateMachine.ChangeState(player.battlePoseState);
    }
}
