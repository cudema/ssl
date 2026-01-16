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
            other.GetComponent<IHealthable>().OnHit(enemy.AttackDamage * damageMultiplier, 0);
        }
    }
}
