using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EE_Melee_02 : NomalEnemyBase
{
    [SerializeField]
    DenggerEffectBase circleDengger;
    [SerializeField]
    DenggerEffectBase squareDengger;

    public override void OnAttack()
    {
        //Debug.Log("attack");
        base.OnAttack();
        switch (currentPattenIndex)
        {
            case 0:
                StartCoroutine(OutpouringOfEvil());
                break;
            case 1:
                StartCoroutine(Rush());
                break;
        }
    }

    [Header("OutpouringOfEvil")]
    [SerializeField]
    Collider outpouringOfEvilAttackCollider;
    [SerializeField]
    float outpouringOfEvilRecoveryTime = 0.1f;

    IEnumerator OutpouringOfEvil()
    {
        isLookAtPlayer = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(41f / 60f));
        circleDengger.Setup(transform.position + Vector3.down, 1.5f, 90f / 60f);
        yield return StartCoroutine(WaitForSecondsOfPertten(90f / 60f));
        outpouringOfEvilAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));
        outpouringOfEvilAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(149f / 60f));

        yield return StartCoroutine(WaitForSecondsOfPertten(outpouringOfEvilRecoveryTime));

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    [Header("Rush")]
    [SerializeField]
    Collider rushAttackCollider;
    [SerializeField]
    float rushRecoveryTime = 0.1f;

    IEnumerator Rush()
    {
        isLookAtPlayer = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(90f / 60f));
        rushAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));
        rushAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(149f / 60f));

        yield return StartCoroutine(WaitForSecondsOfPertten(outpouringOfEvilRecoveryTime));

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }
}
