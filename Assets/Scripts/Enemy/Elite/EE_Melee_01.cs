using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EE_Melee_01 : NomalEnemyBase
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
                StartCoroutine(Flipping());
                break;
            case 1:
                StartCoroutine(Cutdown());
                break;
            case 2:
                StartCoroutine(Bigcut());
                break;
            case 3:
                StartCoroutine(CutAndHit());
                break;
        }
    }

    [Header("Flipping")]
    [SerializeField]
    Collider flippingAttackCollider;
    [SerializeField]
    float flippingRecoveryTime = 0.8f;

    IEnumerator Flipping()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(13f / 60f));
        squareDengger.Setup(transform.position + Vector3.down, 1f, 2f, 40f / 60f);
        yield return StartCoroutine(WaitForSecondsOfPertten(17f / 60f));
        isLookAtPlayer = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(18f / 60f));
        flippingAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        flippingAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(104f / 60f));

        yield return StartCoroutine(WaitForSecondsOfPertten(flippingRecoveryTime));

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    [Header("Cutdown")]
    [SerializeField]
    Collider cutdownAttackCollider;
    [SerializeField]
    float cutdownRecoveryTime = 0.8f;

    IEnumerator Cutdown()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(15f / 60f));
        isLookAtPlayer = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(13f / 60f));
        cutdownAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        cutdownAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(45f / 60f));

        yield return StartCoroutine(WaitForSecondsOfPertten(cutdownRecoveryTime));

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    [Header("Bigcut")]
    [SerializeField]
    Collider bigcutAttackCollider;
    [SerializeField]
    float bigcutRecoveryTime = 0.8f;

    IEnumerator Bigcut()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(35f / 60f));
        isLookAtPlayer = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(15f / 60f));
        bigcutAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(11f / 60f));
        bigcutAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(83f / 60f));

        yield return StartCoroutine(WaitForSecondsOfPertten(bigcutRecoveryTime));

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    [Header("CutAndHit")]
    [SerializeField]
    Collider cutAndHitAttackCollider;
    [SerializeField]
    Collider cutAndHitLastAttackCollider;
    [SerializeField]
    float cutAndHitRecoveryTime = 0.8f;

    IEnumerator CutAndHit()
    {
        yield return StartCoroutine(WaitForSecondsOfPertten(40f / 60f));
        isLookAtPlayer = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(12f / 60f));
        cutAndHitAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(15f / 60f));
        cutAndHitAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(28f / 60f));
        cutAndHitAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        cutAndHitAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(24f / 60f));
        circleDengger.Setup(cutAndHitLastAttackCollider.transform.position + Vector3.down, 1.5f, 39f / 60f);
        yield return StartCoroutine(WaitForSecondsOfPertten(30f / 60f));
        cutAndHitLastAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        cutAndHitLastAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(89f / 60f));

        yield return StartCoroutine(WaitForSecondsOfPertten(cutAndHitRecoveryTime));

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }
}
