using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee02 : NomalEnemyBase
{
    [SerializeField]
    DenggerEffectBase circleDengger;

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
            case 2:
                StartCoroutine(TakeFloor());
                break;
        }
    }

    IEnumerator TwiceCut()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(11f / 60f));
        isLookAtPlayer = false;
        OnAttackMove(8f, 0.5f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(10f / 60f));
        twiceCutAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        twiceCutAttackCollider.enabled = false;
        isLookAtPlayer = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(28f / 60f));
        isLookAtPlayer = false;
        OnAttackMove(8f, 0.5f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        twiceCutAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(0.08f));
        twiceCutAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(twiceCutRecoveryTime));
        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    [Header("DiggingCut")]
    [SerializeField]
    Collider diggingCutAttackCollider;
    [SerializeField]
    float diggingCutRecoveryTime = 1f;

    IEnumerator DiggingCut()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(2f / 60f));
        isLookAtPlayer = false;
        OnAttackMove(16f, 1.5f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(54f / 60f));
        diggingCutAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        diggingCutAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(24f / 60f));
        OnAttackMove(35f, 0.8f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(diggingCutRecoveryTime));
        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    [Header("TakeFloor")]
    [SerializeField]
    Collider takeFloorAttackCollider;
    [SerializeField]
    float takeFloorCutRecoveryTime = 1f;

    IEnumerator TakeFloor()
    {
        isLookAtPlayer = false;
        circleDengger.Setup(transform.position + Vector3.down, 1.5f, 37f / 60f);
        
        yield return StartCoroutine(WaitForSecondsOfPertten(37f / 60f));

        diggingCutAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        diggingCutAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(69f / 60f));

        yield return StartCoroutine(WaitForSecondsOfPertten(diggingCutRecoveryTime));

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    protected override void OnDead()
    {
        base.OnDead();
        
        diggingCutAttackCollider.enabled = false;
        diggingCutAttackCollider.enabled = false;
        twiceCutAttackCollider.enabled = false;
    }
}
