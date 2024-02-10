using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    public Transform playerTarget;

    public EnemyStateMachine stateMachine { get; private set; }

    public string lastAnimBoolName { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        base.Update();
        //stateMachine.currentState.Update();
    }


    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
