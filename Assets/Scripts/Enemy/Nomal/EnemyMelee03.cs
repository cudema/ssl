using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee03 : NomalEnemyBase
{
    [Header("ShieldAttack")]
    [SerializeField]
    Collider shieldAttackAttackCollider;
    [SerializeField]
    float shieldAttackStartupTime = 0.27f;
    [SerializeField]
    float shieldAttackActiveTime = 0.08f;
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
        yield return new WaitForSeconds(shieldAttackStartupTime);
        shieldAttackAttackCollider.enabled = true;
        yield return new WaitForSeconds(shieldAttackActiveTime);
        shieldAttackAttackCollider.enabled = false;
        yield return new WaitForSeconds(shieldAttackRecoveryTime);
        isAttacking = false;
        currentPattenIndex = -1;
    }

    [Header("Cut")]
    [SerializeField]
    Collider cutAttackCollider;
    [SerializeField]
    float cutStartupTime = 0.33f;
    [SerializeField]
    float cutActiveTime = 0.08f;
    [SerializeField]
    float cutRecoveryTime = 0.75f;

    IEnumerator Cut()
    {
        yield return new WaitForSeconds(cutStartupTime);
        cutAttackCollider.enabled = true;
        yield return new WaitForSeconds(cutActiveTime);
        cutAttackCollider.enabled = false;
        yield return new WaitForSeconds(cutRecoveryTime);
        isAttacking = false;
        currentPattenIndex = -1;
    }

    [Header("TwiceCutPoke")]
    [SerializeField]
    Collider twiceCutPokeAttackCollider;
    [SerializeField]
    float twiceCutPokeStartupTime = 0.37f;
    [SerializeField]
    float twiceCutPokeActiveTime = 0.08f;
    [SerializeField]
    float twiceCutPokeRecoveryTime = 1.12f;

    IEnumerator TwiceCutPoke()
    {
        yield return new WaitForSeconds(twiceCutPokeStartupTime);
        twiceCutPokeAttackCollider.enabled = true;
        yield return new WaitForSeconds(twiceCutPokeActiveTime);
        twiceCutPokeAttackCollider.enabled = false;
        yield return new WaitForSeconds(0.4f);
        twiceCutPokeAttackCollider.enabled = true;
        yield return new WaitForSeconds(twiceCutPokeActiveTime);
        twiceCutPokeAttackCollider.enabled = false;
        yield return new WaitForSeconds(0.88f);
        twiceCutPokeAttackCollider.enabled = true;
        yield return new WaitForSeconds(twiceCutPokeActiveTime);
        twiceCutPokeAttackCollider.enabled = false;
        yield return new WaitForSeconds(twiceCutPokeRecoveryTime);
        isAttacking = false;
        currentPattenIndex = -1;
    }
}
