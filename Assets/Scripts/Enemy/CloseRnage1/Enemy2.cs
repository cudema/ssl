using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : EnemyBase
{
    void Start()
    {
        enemyStates[2] = new Enemy2Attack(this, sensingRange, attackRange);
    }
}
