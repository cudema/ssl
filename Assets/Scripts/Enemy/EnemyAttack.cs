using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    Collider attackCollider;
    [SerializeField]
    protected EnemyBase enemy;
    [SerializeField]
    float damageMultiplier;
    [SerializeField]
    float attackRangeDegree = 360;
    [SerializeField]
    AttackStaggerTier staggerTier;

    float rangeDot;

    void Start()
    {
        rangeDot = Mathf.Cos(attackRangeDegree * 0.5f * Mathf.Deg2Rad);
    }

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
        Vector3 direction = (other.transform.position - transform.position).normalized;
        if (Vector3.Dot(transform.forward, direction) < rangeDot) return;
        if (other.CompareTag("Player"))
        {
            IHealthable tempHealthable = other.GetComponent<IHealthable>();
            Debug.Log(tempHealthable);
            tempHealthable.OnHit(enemy.stats.stats[StatType.AttackDamage].Value * damageMultiplier, 0);
            tempHealthable.OnStiffen(staggerTier);
        }
    }
}
