using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : EnemyBase
{
    [SerializeField]
    float trueAttackRange;

    [SerializeField, Header("Attack0")]
    Collider attack0Collider;
    [SerializeField, Header("Attack1")]
    Collider attack1Collider;
    [SerializeField]
    float attack1RushTime = 1f;
    [SerializeField, Header("Attack2")]
    Collider attack2Collider;
    [SerializeField]
    float attack2RushTime = 1f;
    [SerializeField, Header("TempAttack")]
    GameObject tempAttack;

    void Start()
    {
        enemyStates[0] = new Wander(this, sensingRange, attackRange);
        enemyStates[1] = new enemy3Track(this, sensingRange, attackRange);
        enemyStates[2] = new Enemy3Attack(this, sensingRange, attackRange);
    }

    public IEnumerator OnAttack0()
    {
        Debug.Log("OnAttack0");
        tempAttack.SetActive(true);
        movement.LookAtTarget(Player.instance.transform.position);

        while(Vector3.Distance(Player.instance.transform.position, transform.position) > trueAttackRange)
        {
            Vector3 dir = (Player.instance.transform.position - transform.position).normalized;
        
            movement.ToMove(dir);
            PlayMoveAnimation();

            yield return null;
        }

        StopMoveAnimation();

        yield return new WaitForSeconds(1.5f);
        PlayAttackAnimation();
        attack0Collider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        attack0Collider.enabled = false;
        yield return new WaitForSeconds(1f);
        tempAttack.SetActive(false);
    }

    public IEnumerator OnAttack1()
    {
        Debug.Log("OnAttack1");
        tempAttack.SetActive(true);
        Vector3 tempPlayerPos = Player.instance.transform.position;
        float tempTime = 0;


        movement.LookAtTarget(tempPlayerPos);

        yield return new WaitForSeconds(1.5f);
        attack1Collider.enabled = true;

        while (tempTime / attack1RushTime < 1.1)
        {
            transform.position = Vector3.Lerp(transform.position, tempPlayerPos, tempTime / attack1RushTime);
            tempTime += Time.deltaTime;
            yield return null;
        }

        attack1Collider.enabled = false;
        yield return new WaitForSeconds(2f);
        tempAttack.SetActive(false);
    }

    public IEnumerator OnAttack2()
    {
        Debug.Log("OnAttack2");
        tempAttack.SetActive(true);
        Vector3 tempPlayerPos = Player.instance.transform.position;
        float tempTime = 0;

        movement.LookAtTarget(tempPlayerPos);

        yield return new WaitForSeconds(3f);

        while (tempTime / attack2RushTime < 1.1)
        {
            transform.position = Vector3.Slerp(transform.position, tempPlayerPos, tempTime / attack2RushTime);
            tempTime += Time.deltaTime;
            yield return null;
        }

        attack2Collider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        attack2Collider.enabled = false;

        yield return new WaitForSeconds(2f);
        tempAttack.SetActive(false);
    }
}
