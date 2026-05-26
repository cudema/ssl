using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange01 : NomalEnemyBase
{
    [SerializeField]
    DenggerEffectBase circleDengger;

    [Header("Shot")]
    [SerializeField]
    GameObject danger;
    [SerializeField]
    GameObject shotPrefap;
    [SerializeField]
    float shotStartupTime = 1.1f;
    [SerializeField]
    float shotActiveTime = 0.04f;
    [SerializeField]
    float shotRecoveryTime = 0.833f;
    [SerializeField]
    float shotSpeed = 10;

    EnemyRangeAttack bullet;

    IEnumerator Shot()
    {
        danger.SetActive(true);
        yield return StartCoroutine(WaitForSecondsOfPertten(shotStartupTime));

        //발사 타이밍
        isLookAtPlayer = false;
        Vector3 dir = (Player.instance.transform.position - transform.position).normalized * shotSpeed;
        bullet.SetForce(dir, transform, 5f);
        danger.SetActive(false);
        yield return StartCoroutine(WaitForSecondsOfPertten(shotActiveTime));

        yield return StartCoroutine(WaitForSecondsOfPertten(shotRecoveryTime));

        isAttacking = false;
        isLookAtPlayer = true;
        currentPattenIndex = -1;
    }

    public override void OnAttack()
    {
        base.OnAttack();
        //Debug.Log("attack");
        switch (currentPattenIndex)
        {
            case 0:
                bullet = Instantiate(shotPrefap, transform.position, Quaternion.identity).GetComponent<EnemyRangeAttack>();
                bullet.SetEnemy(this);

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
        if (bullet != null) Destroy(bullet.gameObject);
        danger.SetActive(false);

        base.OnDead();
    }
}
