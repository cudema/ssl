using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange02 : NomalEnemyBase
{
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
        float tempTime = Time.time;
        danger.SetActive(true);
        while(Time.time - tempTime < shotStartupTime)
        {
            movement.LookAtTarget(Player.instance.transform.position);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        danger.SetActive(false);
        shotAttackCollider.gameObject.SetActive(true);
        //피격 타이밍
        shotAttackCollider.enabled = true;
        yield return new WaitForSeconds(shotActiveTime);
        shotAttackCollider.enabled = false;

        yield return new WaitForSeconds(0.25f);

        //피격 타이밍
        shotAttackCollider.enabled = true;
        yield return new WaitForSeconds(shotActiveTime);
        shotAttackCollider.enabled = false;

        yield return new WaitForSeconds(0.25f);

        //피격 타이밍
        shotAttackCollider.enabled = true;
        yield return new WaitForSeconds(shotRecoveryTime);
        shotAttackCollider.enabled = false;

        shotAttackCollider.gameObject.SetActive(false);

        isAttacking = false;
        currentPattenIndex = -1;
    }

    public override void OnAttack()
    {
        //Debug.Log("attack");
        base.OnAttack();
        StartCoroutine(Shot());
    }
}
