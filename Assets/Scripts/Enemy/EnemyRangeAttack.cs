using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : EnemyAttack
{
    Vector3 force;
    float tempTime = 0;
    float endTime;

    public override void SetEnemy(EnemyBase enemyBase)
    {
        enemy = enemyBase;
        gameObject.SetActive(false);
    }

    public void SetForce(Vector3 force, Transform transform, float time)
    {
        this.force = force;
        endTime = time;
        gameObject.SetActive(true);
        this.transform.position = transform.position;
    }

    void Update()
    {
        if (tempTime > endTime)
        {
            Destroy(gameObject);
        }
        transform.Translate(force * Time.deltaTime);
        tempTime += Time.deltaTime;
    }
}
