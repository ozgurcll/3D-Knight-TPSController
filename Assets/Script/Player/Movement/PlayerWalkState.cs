using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //HitInfo();

    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        Movement_Dodge_Dash(1.5f);

        if (xInput == 0 && yInput == 0)
            player.stateMachine.ChangeState(player.idleState);









        /* if (Input.GetKeyDown(KeyCode.W))
         {
             HitInfo();
         }

         if (canMove == true)
         {
             newMovePoint = new Vector3(targetMovePoint.x, player.transform.position.y, targetMovePoint.z);
             player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(newMovePoint - player.transform.position), 5 * Time.deltaTime);
             playerMove = player.transform.forward * 3 * Time.deltaTime;

             if (Vector3.Distance(player.transform.position, newMovePoint) <= 0.6f)
             {
                 canMove = false;
             }
         }
         else
             player.stateMachine.ChangeState(player.idleState);*/
    }

}
