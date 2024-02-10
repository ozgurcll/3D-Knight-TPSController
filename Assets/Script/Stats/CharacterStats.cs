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

   /* public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }



        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);


        //if invnteroy current weapon has fire effect
        // then DoMagicalDamage(_targetStats);

    }

    /*private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }*/


    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);
Debug.Log(_damage);
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
