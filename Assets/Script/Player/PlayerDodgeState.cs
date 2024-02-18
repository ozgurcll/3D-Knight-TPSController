using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerState
{
    public PlayerDodgeState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
        player.isDodging = false;
    }
    public override void Update()
    {
        base.Update();

        player.anim.SetFloat("XMove", xInput, 0.3f, Time.deltaTime);
        player.anim.SetFloat("YMove", yInput, 0.3f, Time.deltaTime);
        
        Movement_Dodge_Dash(3);
        
        if (triggerCalled)
        {
            if (player.currentWeaponInHand == null)
                player.stateMachine.ChangeState(player.idleState);
            else
                player.stateMachine.ChangeState(player.battlePoseState);
        }
    }
}
