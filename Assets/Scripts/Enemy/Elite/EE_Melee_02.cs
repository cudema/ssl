using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
        circleDengger.Setup(transform.position + Vector3.down * 1.5f, 1.5f, 90f / 60f);
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
    [SerializeField]
    LayerMask layerMask;

    IEnumerator Rush()
    {

        yield return StartCoroutine(WaitForSecondsOfPertten(140f / 60f));

        isLookAtPlayer = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(30f / 60f));
        OnAttackMove(180f, 90f, false);
        rushAttackCollider.enabled = true;

        yield return new WaitUntil(() => Physics.CheckSphere(transform.position + (movement.renderTransform.forward * 0.7f), 0.7f, layerMask));

        animator.SetTrigger("Well");
        rushAttackCollider.enabled = false;
        StopAttackMove();

        yield return StartCoroutine(WaitForSecondsOfPertten(235f / 60f));
        yield return StartCoroutine(WaitForSecondsOfPertten(outpouringOfEvilRecoveryTime));

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    protected override void OnDead()
    {
        base.OnDead();
        rushAttackCollider.enabled = false;
        outpouringOfEvilAttackCollider.enabled = false;
    }
}
