using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : EnemyAttack
{
    Vector3 force;

    public void SetEnemy(EnemyBase enemyBase)
    {
        enemy = enemyBase;
        gameObject.SetActive(false);
    }

    public void SetForce(Vector3 force)
    {
        this.force = force;
        gameObject.SetActive(true);
        transform.position = enemy.transform.position;
    }

    void Update()
    {
        transform.Translate(force * Time.deltaTime);
    }
}
