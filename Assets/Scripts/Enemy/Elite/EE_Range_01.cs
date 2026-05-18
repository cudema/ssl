using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EE_Range_01 : NomalEnemyBase
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
                StartCoroutine(FlamePillar());
                break;
            case 1:
                StartCoroutine(ExplosionOfPower());
                break;
            case 2:
                StartCoroutine(TorrentOfPower());
                break;
        }
    }

    [Header("FlamePillar")]
    [SerializeField]
    Collider flamePillarAttackCollider;
    [SerializeField]
    float flamePillarRecoveryTime = 0.8f;

    IEnumerator FlamePillar()
    {
        isLookAtPlayer = false;
        flamePillarAttackCollider.transform.position = Player.instance.transform.position;
        circleDengger.Setup(flamePillarAttackCollider.transform.position + Vector3.down, 1.5f, 50f / 60f);
        yield return StartCoroutine(WaitForSecondsOfPertten(50f / 60f));
        flamePillarAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));
        flamePillarAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(65f / 60f));

        yield return StartCoroutine(WaitForSecondsOfPertten(flamePillarRecoveryTime));

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    [Header("ExplosionOfPower")]
    [SerializeField]
    Collider explosionOfPowerAttackCollider;
    [SerializeField]
    float explosionOfPowerRecoveryTime = 0.8f;

    IEnumerator ExplosionOfPower()
    {
        isLookAtPlayer = false;
        circleDengger.Setup(transform.position + Vector3.down, 2.5f, 102f / 60f);
        yield return StartCoroutine(WaitForSecondsOfPertten(102f / 60f));
        explosionOfPowerAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));
        explosionOfPowerAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(61f / 60f));

        yield return StartCoroutine(WaitForSecondsOfPertten(explosionOfPowerRecoveryTime));

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    [Header("TorrentOfPower")]
    [SerializeField]
    GameObject TorrentOfPowerDanger;
    [SerializeField]
    Collider torrentOfPowerAttackCollider;
    [SerializeField]
    float torrentOfPowerTrackingSpeed = 0.3f;
    [SerializeField]
    float torrentOfPowerRecoveryTime = 0.8f;

    IEnumerator TorrentOfPower()
    {
        Coroutine coroutine;

        TorrentOfPowerDanger.SetActive(true);
        yield return StartCoroutine(WaitForSecondsOfPertten(74f / 60f));
        isLookAtPlayer = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(10f / 60f));

        TorrentOfPowerDanger.SetActive(false);
        torrentOfPowerAttackCollider.gameObject.SetActive(true);

        coroutine = StartCoroutine(PlayerTracking());

        //피격 타이밍
        torrentOfPowerAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(27f / 60f));
        torrentOfPowerAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));

        //피격 타이밍
        torrentOfPowerAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(27f / 60f));
        torrentOfPowerAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));

        //피격 타이밍
        torrentOfPowerAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(27f / 60f));
        torrentOfPowerAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));

        //피격 타이밍
        torrentOfPowerAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(27f / 60f));
        torrentOfPowerAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));

        //피격 타이밍
        torrentOfPowerAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(27f / 60f));
        torrentOfPowerAttackCollider.enabled = false;

        torrentOfPowerAttackCollider.gameObject.SetActive(false);
        StopCoroutine(coroutine);

        yield return StartCoroutine(WaitForSecondsOfPertten(81f / 60f));

        yield return StartCoroutine(WaitForSecondsOfPertten(torrentOfPowerRecoveryTime));

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    IEnumerator PlayerTracking()
    {
        while (true)
        {
            LookAtPlayer(torrentOfPowerTrackingSpeed);
            yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));
        }
    }
}
