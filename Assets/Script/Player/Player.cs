using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public GameObject targetEnemy;
    public GameObject trailVFX;
    public bool isBusy { get; private set; }
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool isDodging;
    [HideInInspector] public bool isAttacking;


    public bool isGrounded;
    public bool isInteracting;
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public LayerMask groundLayer;


    #region  Camera System

    [Header("TPS Camera System")]
    Transform cam;
    private float xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    public Transform target;
    Vector3 offset;
    [SerializeField] float follow_smoothing = .1f;
    bool cursorLocked = false;
    [HideInInspector] public bool lockedTarget;

    #endregion

    #region Donning A Sword

    [Header("Donning A Weapon")]
    [SerializeField] public GameObject weaponHolder;
    [SerializeField] public GameObject weapon;
    [SerializeField] public GameObject weaponSheath;
    public GameObject currentWeaponInHand;
    GameObject currentWeaponInSheath;

    #endregion

    #region DashSystem
    [Header("Dash System")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;

    #endregion

    #region Attack Duration

    [Header("Attack Duration")]
    [SerializeField] private float attackCooldown;
    [HideInInspector] public float attackUsageTimer;

    public int comboCounter;
    public float lastTimeAttacked;
    public float comboWindow = 2;
    #endregion

    #region StateMachine

    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkState moveState { get; private set; }
    public PlayerRunState runState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerDodgeState dodgeState { get; private set; }
    public PlayerLandState landState { get; private set; }

    public PlayerDrawSwordState drawSwordState { get; private set; }
    public PlayerSheathSwordState sheathSwordState { get; private set; }

    public PlayerBattlePoseState battlePoseState { get; private set; }
    public PlayerBpWalkState battlePoseRunState { get; private set; }
    public PlayerBpRunState bpRunState { get; private set; }
    public PlayerTargetLockedState targetLockedState { get; private set; }

    public PlayerPrimaryAttackState primaryAttackState { get; private set; }

    public PlayerDeathState deathState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerWalkState(this, stateMachine, "Move");
        runState = new PlayerRunState(this, stateMachine, "Run");

        dashState = new PlayerDashState(this, stateMachine, "Dash");
        dodgeState = new PlayerDodgeState(this, stateMachine, "Dodge");
        landState = new PlayerLandState(this, stateMachine, "Land");

        drawSwordState = new PlayerDrawSwordState(this, stateMachine, "DrawSword");
        sheathSwordState = new PlayerSheathSwordState(this, stateMachine, "SheathSword");

        battlePoseState = new PlayerBattlePoseState(this, stateMachine, "BattlePose");
        battlePoseRunState = new PlayerBpWalkState(this, stateMachine, "BpRun");
        targetLockedState = new PlayerTargetLockedState(this, stateMachine, "TargetLocked");
        bpRunState = new PlayerBpRunState(this, stateMachine, "BP_WTR"); // walk to run

        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");



        deathState = new PlayerDeathState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();

        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main.transform;
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForInput();
        CheckCollision();
    }

    private void FixedUpdate()
    {
        if (!lockedTarget) TPSCamMouseInput(); else stateMachine.ChangeState(targetLockedState);

    }

    #region Camera System
    public void TPSCamMouseInput()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * 10;
        yAxis -= Input.GetAxisRaw("Mouse Y") * 10;
        yAxis = Mathf.Clamp(yAxis, -30, 30);
    }

    public void LookAtTarget()
    {
        transform.localRotation = Quaternion.Euler(0, cam.eulerAngles.y, cam.eulerAngles.z);
        Vector3 r = cam.eulerAngles;
        xAxis = r.y;
        yAxis = 1.8f;
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }
    #endregion
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForInput()
    {
        if (target != null)
        {
            Vector3 target_P = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, target_P, follow_smoothing * Time.deltaTime);
        }

        dashUsageTimer -= Time.deltaTime;
        attackUsageTimer -= Time.deltaTime;

        if (currentWeaponInHand) //If there is a weapon in hand
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && attackUsageTimer < 0 && !isDashing) //Attack Input
            {
                isAttacking = true;
                isRunning = false;
                isDodging = false;
                isDashing = false;
                attackUsageTimer = attackCooldown;
                if (isAttacking && !isRunning && !isDodging && !isDashing)
                {
                    stateMachine.ChangeState(primaryAttackState);
                }
            }
            if (Input.GetKey(KeyCode.LeftShift) && !isDashing && !isDodging && !isAttacking) //BPRun Input
            {
                comboCounter = 0;
                isRunning = true;
                isDodging = false;
                isDashing = false;
                isAttacking = false;
                if (isRunning && !isDodging && !isDashing && !isAttacking)
                    stateMachine.ChangeState(bpRunState);
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift) && !isDashing && !isDodging) //Run Input
        {
            isRunning = true;
            isDodging = false;
            isDashing = false;
            if (isRunning && !isDodging && !isDashing)
                stateMachine.ChangeState(runState);
        }


        if (Input.GetKeyDown(KeyCode.B) && currentWeaponInHand != null)//
            stateMachine.ChangeState(sheathSwordState);                   //
                                                                          //  Sword Drawing Input
        if (Input.GetKeyDown(KeyCode.M) && currentWeaponInHand == null)   //
            stateMachine.ChangeState(drawSwordState);                  //





        if (Input.GetKeyDown(KeyCode.LeftControl) && dashUsageTimer < 0) //Dash Input
        {
            isDashing = true;
            isRunning = false;
            isDodging = false;
            isAttacking = false;
            dashUsageTimer = dashCooldown;
            if (isDashing && !isRunning && !isDodging && !isAttacking)
                stateMachine.ChangeState(dashState);
        }




        if (Input.GetKeyDown(KeyCode.Space) && dashUsageTimer < 0) //Dodge Input
        {
            dashUsageTimer = dashCooldown;
            isDodging = true;
            isRunning = false;
            isDashing = false;
            isAttacking = false;
            if (isDodging && !isRunning && !isDashing && !isAttacking)
            {
                stateMachine.ChangeState(dodgeState);
            }
        }


        if (!lockedTarget) TPSCamMouseInput(); else stateMachine.ChangeState(targetLockedState);


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (cursorLocked)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

            }
        }
    }

    private void CheckCollision()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y += .5f;
        if (!isGrounded)
        {
            if (!isInteracting)
                stateMachine.ChangeState(landState);

            inAirTimer += Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, .2f, -Vector3.up, out hit, groundLayer))
        {
            if (!isGrounded && !isInteracting)
            {
                stateMachine.ChangeState(landState);
            }
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    #region Weapon Equipment?
    public void DrawWeapon()
    {
        currentWeaponInHand = Instantiate(weapon, weaponHolder.transform);
        Destroy(currentWeaponInSheath);
    }

    public void SheathWeapon()
    {
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        Destroy(currentWeaponInHand);
    }
    #endregion

    #region DamageDealer
    public void StartDealDamage()
    {
        if (currentWeaponInHand != null)
            currentWeaponInHand.GetComponentInChildren<DamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        if (currentWeaponInHand != null)
            currentWeaponInHand.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }

    public void CanAttack()
    {
        if (currentWeaponInHand != null)
            currentWeaponInHand.GetComponentInChildren<DamageDealer>().CanAttackPlayer();
    }
    #endregion
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }
}
