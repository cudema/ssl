using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public abstract class EnemyBase : MonoBehaviour, IHealthable
{
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected float defense;
    [SerializeField]
    protected float attackDamage;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float sensingRange;
    [SerializeField]
    protected float attackRange;

    EnemyState currentState;
    protected EnemyState[] enemyStates = new EnemyState[3];

    [HideInInspector]
    public Movement movement;

    StageManager stagemanager;

    [SerializeField]
    Animator animator;

    void Awake()
    {
        movement = GetComponent<Movement>();
        movement.SetSpeed(speed, 10f);

        enemyStates[0] = new Wander(this, sensingRange, attackRange);
        enemyStates[1] = new Track(this, sensingRange, attackRange);
        enemyStates[2] = new Attack(this, sensingRange, attackRange);
    }

    void Start()
    {
        
    }

    public void ChangeState(StateOfEnemy newState)
    {
        currentState.Escape();
        currentState = enemyStates[(int)newState];
        currentState.Start();
    }

    public void Setup(StageManager stagemanager)
    {
        this.stagemanager = stagemanager;

        currentState = enemyStates[0];
        currentState.Start();
    }

    void OnDead()
    {
        stagemanager.AddCountDeadEnemy(this.gameObject);
    }

    public void OnHit(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            OnDead();
        }
    }

    public void PlayMoveAnimation()
    {
        animator.SetBool("isMove", true);
    }

    public void StopMoveAnimation()
    {
        animator.SetBool("isMove", false);
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("attack");
    }
}
