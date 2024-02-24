using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : Enemy
{

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();


    }

    protected override void Update()
    {
        base.Update();
        GetControl();
    }

    private void GetControl()
    {
        if (state == State.Idle)
        {
            Debug.Log("Idle");
            agent.isStopped = true;
            agent.speed = 0f;
            anim.SetBool("Walk", false);


            nextDestination = walkPoints[index].position;
            agent.SetDestination(nextDestination);
        }
        else if (state == State.PATROL)
        {
            Debug.Log("Patrol");
            agent.isStopped = false;
            agent.speed = 5f;
            anim.SetBool("Walk", true);


            nextDestination = walkPoints[index].position;
            agent.SetDestination(nextDestination);
        }
        else if (state == State.CHASE)
        {
            Debug.Log("Chase");

            agent.isStopped = false;
            agent.speed = 3f;
            anim.SetBool("Walk", true);
            agent.SetDestination(playerTarget.position);
        }
        else if (state == State.ATTACK)
        {
            Debug.Log("Attack");
            agent.isStopped = true;
            anim.SetBool("Walk", false);
            agent.speed = 0f;
            Vector3 targetPosition = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position), turnSpeed * Time.deltaTime);

            if (currentAttackTime >= attackRate)
            {
                int index = Random.Range(1, 4);
                anim.SetInteger("Attack", index);
                currentAttackTime = 0;
            }
            else
            {
                currentAttackTime += Time.deltaTime;
            }
        }
        else if (state == State.DEATH)
        {
            Debug.Log("Death");
        }
    }
}
