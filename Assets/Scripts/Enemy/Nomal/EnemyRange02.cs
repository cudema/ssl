using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange02 : NomalEnemyBase
{
    [SerializeField]
    DenggerEffectBase circleDengger;

    [Header("Shot")]
    [SerializeField]
    GameObject danger;
    [SerializeField]
    Collider shotAttackCollider;
    [SerializeField]
    float shotStartupTime = 1.5f;
    [SerializeField]
    float shotActiveTime = 0.067f;
    [SerializeField]
    float shotRecoveryTime = 0.833f;

    IEnumerator Shot()
    {        
        danger.SetActive(true);

        yield return StartCoroutine(WaitForSecondsOfPertten(shotStartupTime));
        isLookAtPlayer = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(0.1f));

        danger.SetActive(false);
        shotAttackCollider.gameObject.SetActive(true);
        //피격 타이밍
        shotAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(3f / 60f));
        shotAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(0.183f));

        //피격 타이밍
        shotAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(3f / 60f));
        shotAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(0.183f));

        //피격 타이밍
        shotAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(3f / 60f));
        shotAttackCollider.enabled = false;

        shotAttackCollider.gameObject.SetActive(false);

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    public override void OnAttack()
    {
        //Debug.Log("attack");
        base.OnAttack();

        switch (currentPattenIndex)
        {
            case 0:
                StartCoroutine(Shot());
                break;
            case 1:
                StartCoroutine(FlamePillar());
                break;
        }
    }

    [Header("FlamePillar")]
    [SerializeField]
    EnemyAttack flamePillarAttackCollider;
    [SerializeField]
    float flamePillarRecoveryTime = 0.8f;

    IEnumerator FlamePillar()
    {
        isLookAtPlayer = false;
        flamePillarAttackCollider.transform.position = Player.instance.transform.position;
        circleDengger.Setup(flamePillarAttackCollider.transform.position + Vector3.down, 1f, 62f / 60f);
        yield return StartCoroutine(WaitForSecondsOfPertten(62f / 60f));
        flamePillarAttackCollider.OnAttack();
        yield return StartCoroutine(WaitForSecondsOfPertten(8f / 60f));
        flamePillarAttackCollider.OffAttack();
        yield return StartCoroutine(WaitForSecondsOfPertten(49f / 60f));

        yield return StartCoroutine(WaitForSecondsOfPertten(flamePillarRecoveryTime));

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    protected override void OnDead()
    {
        base.OnDead();

        shotAttackCollider.enabled = false;
        flamePillarAttackCollider.OffAttack();
        danger.SetActive(false);
    }
}
