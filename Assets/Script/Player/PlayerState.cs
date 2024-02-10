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

    #region PlayerMoveState
    protected Vector3 newMovePoint;
    protected Vector3 playerMove = Vector3.zero;
    protected bool canMove;
    protected float playerToPointDistance;

    protected Vector3 targetMovePoint = Vector3.zero;
    protected Vector3 TargetPosition { get { return targetMovePoint; } set { targetMovePoint = value; } }

    #endregion

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

    public void HitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            playerToPointDistance = Vector3.Distance(player.transform.position, hit.point);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (playerToPointDistance >= 1f)
                {
                    canMove = true;
                    canAttackMove = false;
                    targetMovePoint = hit.point;
                }
            }
        }
    }

    public void Ability(float _abilitySpeed)
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 dashAbility = player.transform.forward * _abilitySpeed;
            rb.AddForce(dashAbility, ForceMode.Impulse);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Vector3 dashAbility = player.transform.forward * -_abilitySpeed;
            rb.AddForce(dashAbility, ForceMode.Impulse);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Vector3 dashAbility = player.transform.right * _abilitySpeed;
            rb.AddForce(dashAbility, ForceMode.Impulse);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Vector3 dashAbility = player.transform.right * -_abilitySpeed;
            rb.AddForce(dashAbility, ForceMode.Impulse);
        }
    }

    public void Movement()
    {
        Vector3 direction = new Vector3(xInput, 0f, yInput).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, .1f);
            player.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            player.anim.SetFloat("XMove", xInput, 0.1f, Time.deltaTime);
            player.anim.SetFloat("YMove", yInput, 0.1f, Time.deltaTime);
        }
    }
}
