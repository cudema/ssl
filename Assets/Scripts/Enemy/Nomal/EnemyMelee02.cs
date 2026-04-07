using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee02 : NomalEnemyBase
{
    [Header("TwiceCut")]
    [SerializeField]
    Collider twiceCutAttackCollider;
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
        yield return StartCoroutine(WaitForSecondsOfPertten(16f / 60f));
        LookAtPlayer();
        OnAttackMove(8f / 60f, 0.5f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(10f / 60f));
        twiceCutAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(0.08f));
        twiceCutAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(40f / 60f));
        LookAtPlayer();
        OnAttackMove(8f / 60f, 0.5f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        twiceCutAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(0.08f));
        twiceCutAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(twiceCutRecoveryTime));
        isAttacking = false;
        currentPattenIndex = -1;
    }

    [Header("DiggingCut")]
    [SerializeField]
    Collider diggingCutAttackCollider;
    [SerializeField]
    float diggingCutRecoveryTime = 1f;

    IEnumerator DiggingCut()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(3f / 60f));
        LookAtPlayer();
        OnAttackMove(16f / 60f, 1.5f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(54f / 60f));
        diggingCutAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(0.08f));
        diggingCutAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(26f / 60f));
        OnAttackMove(35f / 60f, 0.8f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(diggingCutRecoveryTime));
        isAttacking = false;
        currentPattenIndex = -1;
    }
}
