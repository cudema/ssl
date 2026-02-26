using System.Collections;
using UnityEngine;

public class Boss0Wander : EnemyState
{
    public Boss0Wander(EnemyBase enemy, float sensingRange, float attackRange) : base(enemy, sensingRange, attackRange)
    {

    }

    public override void Start()
    {
        
        coroutine = enemy.StartCoroutine(Progress());
    }

    public override IEnumerator Progress()
    {
        Vector3 dir = Vector3.zero;

        while (true)
        {
            if ((int)Time.time % 8 == 0)
            {
                enemy.StopMoveAnimation();
                dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                dir = dir.normalized;
                yield return new WaitForSeconds(1f);
                enemy.PlayMoveAnimation();
            }

            enemy.movement.ToMove(dir);

            yield return null;

            if (Vector3.Distance(Player.instance.transform.position, enemy.transform.position) < sRange)
            {
                enemy.ChangeState(StateOfEnemy.Track);
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public override void Escape()
    {
        enemy.StopCoroutine(coroutine);

        enemy.StopMoveAnimation();
    }
}

public class Boss0Attack : EnemyState
{
    FerociousTerms fEnemy;

    public Boss0Attack(FerociousTerms enemy, float sensingRange, float attackRange) : base(enemy, sensingRange, attackRange)
    {
        this.fEnemy = enemy;
    }

    public override void Start()
    {
        
        coroutine = enemy.StartCoroutine(Progress());
    }

    public override IEnumerator Progress()
    {
        while (true)
        {
            Vector3 dir = (Player.instance.transform.position - enemy.transform.position).normalized;
        
            enemy.movement.ToMove(dir);
            enemy.PlayMoveAnimation();

            yield return null;

            float distanceToPlayer = Vector3.Distance(Player.instance.transform.position, enemy.transform.position);

            // if (Vector3.Distance(Player.instance.transform.position, enemy.transform.position) > sRange)
            // {
            //     enemy.StopMoveAnimation();
            //     enemy.ChangeState(StateOfEnemy.Wander);
            //     yield return new WaitForSeconds(1f);
            // }

            if (distanceToPlayer < aRange)
            {
                yield return new WaitUntil(() => fEnemy.ChackPatten());
            }

            if (fEnemy.isPattern)
            {
                fEnemy.isPattern = false;

                enemy.ChangeState(StateOfEnemy.Alert);

                yield return new WaitForSeconds(1f);
            }
        }
    }

    public override void Escape()
    {
        enemy.StopCoroutine(coroutine);

        enemy.StopMoveAnimation();
    }
}