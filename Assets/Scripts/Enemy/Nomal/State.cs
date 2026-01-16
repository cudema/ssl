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
            
            enemy.PlayAttackAnimation();
            enemy.movement.LookAtTarget(PlayerWeapon.instance.transform.position);

            yield return new WaitForSeconds(enemy.GetAttackTime());

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

public class Enemy0Attack : Attack
{
    public Enemy0Attack(EnemyBase enemy, float sensingRange, float attackRange) : base(enemy, sensingRange, attackRange)
    {

    }

    public override IEnumerator Progress()
    {
        while (true)
        {
            
            enemy.PlayAttackAnimation();
            enemy.movement.LookAtTarget(PlayerWeapon.instance.transform.position);

            yield return new WaitForSeconds(enemy.GetAttackTime());

            if (Vector3.Distance(PlayerWeapon.instance.transform.position, enemy.transform.position) > aRange)
            {
                enemy.ChangeState(StateOfEnemy.Track);
            }
        }
    }
}

public class Enemy1Attack : Attack
{
    public Enemy1Attack(EnemyBase enemy, float sensingRange, float attackRange) : base(enemy, sensingRange, attackRange)
    {

    }

    public override IEnumerator Progress()
    {
        while (true)
        {
            
            enemy.PlayAttackAnimation();
            enemy.movement.LookAtTarget(PlayerWeapon.instance.transform.position);

            yield return new WaitForSeconds(enemy.GetAttackTime());

            if (Vector3.Distance(PlayerWeapon.instance.transform.position, enemy.transform.position) > aRange)
            {
                enemy.ChangeState(StateOfEnemy.Track);
            }
        }
    }
}

public class Enemy2Attack : Attack
{
    public Enemy2Attack(EnemyBase enemy, float sensingRange, float attackRange) : base(enemy, sensingRange, attackRange)
    {

    }

    public override IEnumerator Progress()
    {
        while (true)
        {
            enemy.PlayAttackAnimation();
            enemy.movement.LookAtTarget(PlayerWeapon.instance.transform.position);

            yield return new WaitForSeconds(enemy.GetAttackTime());

            if (Vector3.Distance(PlayerWeapon.instance.transform.position, enemy.transform.position) > aRange)
            {
                enemy.ChangeState(StateOfEnemy.Track);
            }
        }
    }
}

public class enemy3Track : Track
{
    int tempSkillCount = 0;
    Enemy3 enemy3;

    public enemy3Track(Enemy3 enemy, float sensingRange, float attackRange) : base(enemy, sensingRange, attackRange)
    {
        enemy3 = enemy;
    }

    public override IEnumerator Progress()
    {
        while (true)
        {
            // Vector3 dir = (PlayerWeapon.instance.transform.position - enemy.transform.position).normalized;
        
            // enemy.movement.ToMove(dir);
            // enemy.PlayMoveAnimation();

            if (tempSkillCount % 2 == 1)
            {
                yield return enemy3.StartCoroutine(enemy3.OnAttack1());
                tempSkillCount++;
            }
            else
            {
                yield return enemy3.StartCoroutine(enemy3.OnAttack2());
                tempSkillCount++;
            }

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
}

public class Enemy3Attack : Attack
{
    Enemy3 enemy3;
    public Enemy3Attack(Enemy3 enemy, float sensingRange, float attackRange) : base(enemy, sensingRange, attackRange)
    {
        enemy3 = enemy;
    }

    public override IEnumerator Progress()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            yield return enemy3.StartCoroutine(enemy3.OnAttack0());

            if (Vector3.Distance(PlayerWeapon.instance.transform.position, enemy.transform.position) > aRange)
            {
                enemy.ChangeState(StateOfEnemy.Track);
            }
        }
    }
}
