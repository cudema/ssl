using System.Collections;
using UnityEngine;

public abstract class EnemyState
{
    protected EnemyBase enemy;
    protected float sRange;
    protected float aRange;

    protected Coroutine coroutine;

    public EnemyState(EnemyBase enemy, float sensingRange, float attackRange)
    {
        this.enemy = enemy;
        sRange = sensingRange;
        aRange = attackRange;
    }

    public abstract void Start();

    public abstract IEnumerator Progress();

    public abstract void Escape();
}
