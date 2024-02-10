using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitTarget;

    public void HitVFX(Vector3 hitPosition)
    {
        ParticleSystem hit = Instantiate(hitTarget, hitPosition, Quaternion.identity);
        Destroy(hit.gameObject, 3f);
    }
}
