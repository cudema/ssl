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
        //Debug.Log("attack");
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
        yield return StartCoroutine(WaitForSecondsOfPertten(twiceCutStartupTime));
        twiceCutAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(twiceCutActiveTime));
        twiceCutAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(40f / 60f));
        twiceCutAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(twiceCutActiveTime));
        twiceCutAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(twiceCutRecoveryTime));
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
    float diggingCutRecoveryTime = 1f;

    IEnumerator DiggingCut()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(diggingCutStartupTime));
        diggingCutAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(diggingCutActiveTime));
        diggingCutAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(diggingCutRecoveryTime));
        isAttacking = false;
        currentPattenIndex = -1;
    }
}
