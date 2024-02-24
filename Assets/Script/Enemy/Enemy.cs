using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    public enum State
    {
        Idle,
        PATROL,
        CHASE,
        ATTACK,
        DEATH
    }
    [Header("State")]
    public State bossState = State.Idle;
    public State state { get { return bossState; } }

    [Header("Donning A Weapon")]
    [SerializeField] public GameObject weaponHolder;
    [SerializeField] public GameObject weapon;
    public GameObject currentWeaponInHand;

    [Header("Target")]
    public Transform playerTarget;
    public Transform[] walkPoints;
    private float distanceToTarget;
    public Vector3 nextDestination;


    [Header("Movement Settings")]
    public float turnSpeed = 5f;
    public float patrolTime = 10f;
    public float walkDistance = 8f;

    [Header("Attack Settings")]
    public float currentAttackTime;
    public float attackDistance = 2f;
    public float attackRate = 1f;



    public NavMeshAgent agent;
    public int index;
    protected override void Awake()
    {
        base.Awake();

        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        currentWeaponInHand = Instantiate(weapon, weaponHolder.transform);

        index = Random.Range(0, walkPoints.Length);
        if (walkPoints.Length > 0)
        {
            InvokeRepeating("Patrol", Random.Range(0, patrolTime), patrolTime);
        }

    }

    protected override void Update()
    {
        base.Update();
        SetState();
    }

    void SetState()
    {
        distanceToTarget = Vector3.Distance(playerTarget.position, transform.position);
        //int enemyCount = FindObjectsOfType<EnemyStats>().Length;
        if (distanceToTarget > walkDistance)
        {
            if (agent.remainingDistance >= agent.stoppingDistance)
            {
                bossState = State.PATROL;


            }
            else
            {
                bossState = State.Idle;


            }
        }
        else
        {
            if (distanceToTarget > attackDistance + 0.15)
            {
                if (!anim.IsInTransition(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    bossState = State.CHASE;
                }

                agent.isStopped = false;
                agent.speed = 3f;
                anim.SetBool("Walk", true);
                agent.SetDestination(playerTarget.position);
            }
            else if (distanceToTarget <= attackDistance)
            {
                CanAttack();
                bossState = State.ATTACK;

            }
        }

    }

    /*if (currentHealth <= 0f)
    {
        bossState = State.DEATH;
    }*/

    void Patrol()
    {
        index = index == walkPoints.Length - 1 ? 0 : index + 1;
    }


    #region DamageDealer
    public void StartDealDamage()
    {
        currentWeaponInHand.GetComponentInChildren<DamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        currentWeaponInHand.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }

    public void CanAttack()
    {
        currentWeaponInHand.GetComponentInChildren<DamageDealer>().CanAttackEnemy();
    }
    #endregion



}
