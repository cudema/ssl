using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected float distane;
    [SerializeField]
    protected float attackDamage;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float sensingRange;
    [SerializeField]
    protected float attackRange;

    EnemyState currentState;
    EnemyState[] enemyStates = new EnemyState[3];

    public Movement movement;

    void Awake()
    {
        movement = GetComponent<Movement>();
        movement.SetSpeed(speed, 0.2f);

        enemyStates[0] = new Wander(this, sensingRange, attackRange);
        enemyStates[1] = new Track(this, sensingRange, attackRange);
        enemyStates[2] = new Attack(this, sensingRange, attackRange);
        currentState = enemyStates[0];
    }

    void Start()
    {
        currentState.Start();
    }

    public void ChangeState(StateOfEnemy newState)
    {
        currentState.Escape();
        currentState = enemyStates[(int)newState];
        currentState.Start();
    }
}
