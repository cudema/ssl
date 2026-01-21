using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    Collider attackCollider;
    [SerializeField]
    EnemyBase enemy;
    [SerializeField]
    float damageMultiplier;
    [SerializeField]
    AttackStaggerTier staggerTier;

    public void OnAttack()
    {
        attackCollider.enabled = true;
    }

    public void OffAttack()
    {
        attackCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IHealthable tempHealthable = other.GetComponent<IHealthable>();
            tempHealthable.OnHit(enemy.AttackDamage * damageMultiplier, 0);
            tempHealthable.OnStiffen(staggerTier);
        }
    }
}
