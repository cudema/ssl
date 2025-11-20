using System.Collections;
using UnityEngine;

public enum StateOfEnemy { Wander = 0, Track, Attack }

public class Wander : EnemyState
{
    public Wander(EnemyBase enemy, float sensingRange, float attackRange) : base(enemy, sensingRange, attackRange)
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

            if (Vector3.Distance(PlayerWeapon.instance.transform.position, enemy.transform.position) < sRange)
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

public class Track : EnemyState
{
    public Track(EnemyBase enemy, float sensingRange, float attackRange) : base(enemy, sensingRange, attackRange)
    {

    }

    public override void Start()
    {


        coroutine = enemy.StartCoroutine(Progress());
    }

    public override IEnumerator Progress()
    {
        while (true)
        {
            Vector3 dir = (PlayerWeapon.instance.transform.position - enemy.transform.position).normalized;
        
            enemy.movement.ToMove(dir);
            enemy.PlayMoveAnimation();

            yield return null;

            if (Vector3.Distance(PlayerWeapon.instance.transform.position, enemy.transform.position) > sRange)
            {
                enemy.StopMoveAnimation();
                enemy.ChangeState(StateOfEnemy.Wander);
                yield return new WaitForSeconds(1f);

            }

            if (Vector3.Distance(PlayerWeapon.instance.transform.position, enemy.transform.position) < aRange)
            {
                enemy.StopMoveAnimation();
                enemy.ChangeState(StateOfEnemy.Attack);
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

public class Attack : EnemyState
{
    public Attack(EnemyBase enemy, float sensingRange, float attackRange) : base(enemy, sensingRange, attackRange)
    {

    }

    public override void Start()
    {


        coroutine = enemy.StartCoroutine(Progress());
    }

    public override IEnumerator Progress()
    {
        while (true)
        {

            yield return null;

            if (Vector3.Distance(PlayerWeapon.instance.transform.position, enemy.transform.position) > aRange)
            {
                enemy.ChangeState(StateOfEnemy.Track);
            }
        }
    }

    public override void Escape()
    {
        enemy.StopCoroutine(coroutine);


    }
}
