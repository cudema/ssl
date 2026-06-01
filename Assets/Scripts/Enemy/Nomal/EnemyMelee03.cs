using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee03 : NomalEnemyBase
{
    [SerializeField]
    DenggerEffectBase squareDengger;

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
            case 3:
                StartCoroutine(RushShoving());
                break;
        }
    }

    IEnumerator ShieldAttack()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(10f / 60f));
        isLookAtPlayer = false;
        OnAttackMove(9f, 0.8f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        shieldAttackAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        shieldAttackAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(9f / 60f));
        OnAttackMove(31f, 0.6f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(shieldAttackRecoveryTime));
        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    [Header("Cut")]
    [SerializeField]
    Collider cutAttackCollider;
    [SerializeField]
    float cutRecoveryTime = 0.75f;

    IEnumerator Cut()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        isLookAtPlayer = false;
        OnAttackMove(10f, 0.7f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(10f / 60f));
        cutAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        cutAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(10f / 60f));
        OnAttackMove(34f, 0.6f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(cutRecoveryTime));
        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    [Header("TwiceCutPoke")]
    [SerializeField]
    Collider twiceCutPokeAttackCollider;
    [SerializeField]
    float twiceCutPokeRecoveryTime = 1.12f;

    IEnumerator TwiceCutPoke()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));
        isLookAtPlayer = false;
        OnAttackMove(14f, 0.5f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(17f / 60f));
        twiceCutPokeAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(0.08f));
        twiceCutPokeAttackCollider.enabled = false;
        isLookAtPlayer = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(23f / 60f));
        isLookAtPlayer = false;
        OnAttackMove(4f, 0.5f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));
        twiceCutPokeAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(0.08f));
        twiceCutPokeAttackCollider.enabled = false;
        isLookAtPlayer = true;

        yield return StartCoroutine(WaitForSecondsOfPertten(50f / 60f));
        isLookAtPlayer = false;
        OnAttackMove(8f, 2f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(3f / 60f));

        twiceCutPokeAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(0.08f));
        twiceCutPokeAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(twiceCutPokeRecoveryTime));
        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    [Header("RushShoving")]
    [SerializeField]
    Collider rushShovingAttackCollider;
    [SerializeField]
    float rushShovingRecoveryTime = 0.8f;

    IEnumerator RushShoving()
    {
        squareDengger.Setup(transform.position + Vector3.down, 1f, 1.5f, 50f / 60f);
        isLookAtPlayer = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(50f / 60f));

        rushShovingAttackCollider.enabled = true;
        OnAttackMove(5f, 1.5f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        rushShovingAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(24f / 60f));
        OnAttackMove(40f, 0.5f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(40f / 60f));


        yield return StartCoroutine(WaitForSecondsOfPertten(rushShovingRecoveryTime));
        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    protected override void ChangedHP()
    {
        base.ChangedHP();
        shieldAttackAttackCollider.enabled = false;
        rushShovingAttackCollider.enabled = false;
        twiceCutPokeAttackCollider.enabled = false;
        cutAttackCollider.enabled = false;
    }
}
