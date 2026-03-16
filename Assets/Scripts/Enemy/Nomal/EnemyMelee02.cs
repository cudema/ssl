using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee02 : NomalEnemyBase
{
    [Header("TwiceCut")]
    [SerializeField]
    Collider twiceCutAttackCollider;
    [SerializeField]
    float twiceCutStartupTime = 0.37f;
    [SerializeField]
    float twiceCutActiveTime = 0.08f;
    [SerializeField]
    float twiceCutRecoveryTime = 0.8f;

    public override void OnAttack()
    {
        Debug.Log("attack");
        base.OnAttack();
        switch (currentPattenIndex)
        {
            case 0:
                StartCoroutine(TwiceCut());
                break;
            case 1:
                StartCoroutine(DiggingCut());
                break;
        }
    }

    IEnumerator TwiceCut()
    {
        yield return new WaitForSeconds(twiceCutStartupTime);
        twiceCutAttackCollider.enabled = true;
        yield return new WaitForSeconds(twiceCutActiveTime);
        twiceCutAttackCollider.enabled = false;
        yield return new WaitForSeconds(40f / 60f);
        twiceCutAttackCollider.enabled = true;
        yield return new WaitForSeconds(twiceCutActiveTime);
        twiceCutAttackCollider.enabled = false;
        yield return new WaitForSeconds(twiceCutRecoveryTime);
        isAttacking = false;
        currentPattenIndex = -1;
    }

    [Header("DiggingCut")]
    [SerializeField]
    Collider diggingCutAttackCollider;
    [SerializeField]
    float diggingCutStartupTime = 0.92f;
    [SerializeField]
    float diggingCutActiveTime = 0.08f;
    [SerializeField]
    float diggingCutRecoveryTime = 0.98f;

    IEnumerator DiggingCut()
    {
        yield return new WaitForSeconds(twiceCutStartupTime);
        diggingCutAttackCollider.enabled = true;
        yield return new WaitForSeconds(twiceCutActiveTime);
        diggingCutAttackCollider.enabled = true;
        yield return new WaitForSeconds(twiceCutRecoveryTime);
        isAttacking = false;
        currentPattenIndex = -1;
    }
}
