using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawSwordState : PlayerState
{
    public PlayerDrawSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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

        if (player.currentWeaponInHand == null)
            player.stateMachine.ChangeState(player.battlePoseState);
        else
            player.stateMachine.ChangeState(player.idleState);


    }
}
