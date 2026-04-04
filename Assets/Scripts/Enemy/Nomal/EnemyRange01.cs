using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange01 : NomalEnemyBase
{
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
        float tempTime = Time.time;
        danger.SetActive(true);
        while(Time.time - tempTime < shotStartupTime)
        {
            movement.LookAtTarget(Player.instance.transform.position);
            yield return null;
        }

        //발사 타이밍
        Vector3 dir = (Player.instance.transform.position - transform.position).normalized * shotSpeed;
        bullet.SetForce(dir);
        danger.SetActive(false);
        yield return StartCoroutine(WaitForSecondsOfPertten(shotActiveTime));

        yield return StartCoroutine(WaitForSecondsOfPertten(shotRecoveryTime));

        isAttacking = false;
        currentPattenIndex = -1;
    }

    public override void OnAttack()
    {
        //Debug.Log("attack");
        base.OnAttack();
        bullet = Instantiate(shotPrefap, transform.position, Quaternion.identity).GetComponent<EnemyRangeAttack>();
        bullet.SetEnemy(this);

        StartCoroutine(Shot());
    }
}
