using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    bool canDealDamage;
    List<GameObject> hasDealtDamage;

    CameraShake cameraShake;
    [SerializeField] float weaponLength;
    [SerializeField] float weaponDamage;
    void Start()
    {
        cameraShake = CameraShake.Instance;
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }

    public void CanAttackPlayer()
    {
        if (canDealDamage)
        {
            RaycastHit hit;

            int layerMask = 1 << 7;
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out Enemy enemy) && !hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    cameraShake.ShakeCamera(2f, .2f);
                    enemy.stats.TakeDamage(10);
                    enemy.fx.HitVFX(hit.point);
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }
        }
    }

    public void CanAttackEnemy()
    {
        if (canDealDamage)
        {
            RaycastHit hit;

            int layerMask = 1 << 9;
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out Player player) && !hasDealtDamage.Contains(hit.transform.gameObject))
                {
                  //  Debug.Log("Hit Player");
                    cameraShake.ShakeCamera(2f, .2f);
                    player.stats.TakeDamage(10);
                    player.fx.HitVFX(hit.point);
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage.Clear();
    }
    public void EndDealDamage()
    {
        cameraShake.ShakeCamera(0f, 1f);
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.forward * weaponLength);
    }
}
