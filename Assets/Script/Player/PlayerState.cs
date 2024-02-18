using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine playerStateMachine;
    protected Player player;

    protected Rigidbody rb;
    protected bool triggerCalled;
    protected bool canAttackMove;
    protected float turnSmoothVelocity;

    protected float xInput;
    protected float yInput;



    protected float stateTimer;

    private string animBoolName;

    public PlayerState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName)
    {
        this.player = _player;
        this.playerStateMachine = _playerStateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        //Debug.Log("I m in: " + animBoolName);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger() => triggerCalled = true;

    public void Movement_Dodge_Dash(float _speed)
    {
        if (!player.isGrounded) return;

        Vector3 direction = new Vector3(xInput, 0f, yInput).normalized;
        direction = player.transform.TransformDirection(direction);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, .1f);
            player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            player.transform.position += direction * _speed * Time.deltaTime;
            player.anim.SetFloat("XMove", xInput, 0.1f, Time.deltaTime);
            player.anim.SetFloat("YMove", yInput, 0.1f, Time.deltaTime);
        }
    }
}
