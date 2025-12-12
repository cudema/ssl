using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    void Start()
    {
        enemyStates[2] = new Enemy1Attack(this, sensingRange, attackRange);
    }
}
