using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee01 : NomalEnemyBase
{
    [Header("Stabbing")]
    [SerializeField]
    Collider stabbingAttackCollider;
    [SerializeField]
    float stabbingRecoveryTime = 1f;

    public override void OnAttack()
    {
        base.OnAttack();
        //Debug.Log("OnAttack");
        StartCoroutine(Stabbing());
    }

    IEnumerator Stabbing()
    {
        isLookAtPlayer = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(55f / 60f));
        stabbingAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        stabbingAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(stabbingRecoveryTime));
        isAttacking = false;
        isLookAtPlayer = true;
    }
}
