using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody rb { get; private set; }
    public CapsuleCollider cc { get; private set; }

    public EntityFX fx { get; private set; }


    public CharacterStats stats { get; private set; }

    protected bool isKnocked;





    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        fx = GetComponent<EntityFX>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        stats = GetComponent<CharacterStats>();
    }
    protected virtual void Update()
    {

    }

    public void SetZeroVelocity()
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector3(0, 0, 0);
    }

    public void SetVelocity(float _xVelocity, float _zVelocity)
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector3(_xVelocity, rb.velocity.y, _zVelocity);
    }

    public virtual void Die()
    {
        Destroy(this.gameObject, 2.5f);
    }

}
