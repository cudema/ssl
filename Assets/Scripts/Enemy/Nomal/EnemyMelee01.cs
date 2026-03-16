using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee01 : NomalEnemyBase
{
    [Header("Stabbing")]
    [SerializeField]
    Collider stabbingAttackCollider;
    [SerializeField]
    float stabbingStartupTime = 0.92f;
    [SerializeField]
    float stabbingActiveTime = 0.08f;
    [SerializeField]
    float stabbingRecoveryTime = 1f;

    public override void OnAttack()
    {
        base.OnAttack();
        Debug.Log("OnAttack");
        StartCoroutine(Stabbing());
    }

    IEnumerator Stabbing()
    {
        yield return new WaitForSeconds(stabbingStartupTime);
        stabbingAttackCollider.enabled = true;
        yield return new WaitForSeconds(stabbingActiveTime);
        stabbingAttackCollider.enabled = false;
        yield return new WaitForSeconds(stabbingRecoveryTime);
        isAttacking = false;
    }
}
