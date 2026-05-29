using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : EnemyBase
{
    void Start()
    {
        enemyStates[2] = new Attack(this, sensingRange, attackRange);
        enemyStates[3] = new Alert(this, sensingRange, attackRange);
        StopMoveAnimation();
    }
}
