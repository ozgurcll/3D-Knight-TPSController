using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    public Stat damage;
    public Stat maxHealth;
    public int currentHealth;
    protected bool isDead;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();

        fx = GetComponent<EntityFX>();

    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);
        if (currentHealth <= 0 && !isDead)
            Die();
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
    }

    protected virtual void Die()
    {
        isDead = true;
    }

}
