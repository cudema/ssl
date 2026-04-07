using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee03 : NomalEnemyBase
{
    [Header("ShieldAttack")]
    [SerializeField]
    Collider shieldAttackAttackCollider;
    [SerializeField]
    float shieldAttackRecoveryTime = 0.52f;

    public override void OnAttack()
    {
        //Debug.Log("attack");
        base.OnAttack();
        switch (currentPattenIndex)
        {
            case 0:
                StartCoroutine(ShieldAttack());
                break;
            case 1:
                StartCoroutine(Cut());
                break;
            case 2:
                StartCoroutine(TwiceCutPoke());
                break;
        }
    }

    IEnumerator ShieldAttack()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(14f / 60f));
        LookAtPlayer();
        OnAttackMove(9f / 60f, 0.8f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        shieldAttackAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(0.08f));
        shieldAttackAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(9f / 60f));
        OnAttackMove(31f / 60f, 0.6f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(shieldAttackRecoveryTime));
        isAttacking = false;
        currentPattenIndex = -1;
    }

    [Header("Cut")]
    [SerializeField]
    Collider cutAttackCollider;
    [SerializeField]
    float cutRecoveryTime = 0.75f;

    IEnumerator Cut()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(14f / 60f));
        LookAtPlayer();
        OnAttackMove(10f / 60f, 0.7f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(10f / 60f));
        cutAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(0.08f));
        cutAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(10f / 60f));
        OnAttackMove(34f / 60f, 0.6f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(cutRecoveryTime));
        isAttacking = false;
        currentPattenIndex = -1;
    }

    [Header("TwiceCutPoke")]
    [SerializeField]
    Collider twiceCutPokeAttackCollider;
    [SerializeField]
    float twiceCutPokeRecoveryTime = 1.12f;

    IEnumerator TwiceCutPoke()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(8f / 60f));
        LookAtPlayer();
        OnAttackMove(14f / 60f, 0.5f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(17f / 60f));
        twiceCutPokeAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(0.08f));
        twiceCutPokeAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(23f / 60f));
        LookAtPlayer();
        OnAttackMove(4f / 60f, 0.5f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));
        twiceCutPokeAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(0.08f));
        twiceCutPokeAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(50f / 60f));
        LookAtPlayer();
        OnAttackMove(8f / 60f, 2f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(3f / 60f));

        twiceCutPokeAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(0.08f));
        twiceCutPokeAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(twiceCutPokeRecoveryTime));
        isAttacking = false;
        currentPattenIndex = -1;
    }
}
