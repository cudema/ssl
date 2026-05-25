using System.Collections;
using UnityEngine;

public class UnleashedDemon : EnemyBase
{
    [SerializeField]
    DenggerEffectBase circleDengger;    
    [SerializeField]
    DenggerEffectBase squareDengger;

    bool isBerserk = false;
    Coroutine currentPatten = null;

    bool isBackJump = false;

    [SerializeField]
    float shotDeshSpeed;

    [HideInInspector]
    public bool isPattern = false;

    void Start()
    {
        enemyStates[2] = new Boss1Attack(this, sensingRange, attackRange);
        enemyStates[3] = new Alert0(this, sensingRange, attackRange);
        currentState = enemyStates[0];
        hp = stats.stats[StatType.HP].Value;
        StartCoroutine(IsBackJump());
    }

    public override void Setup(StageManager stagemanager)
    {
        base.Setup(stagemanager);
    }

    bool jump75 = true;
    bool jump25 = true;

    protected override void ChangedHP()
    {
        base.ChangedHP();
        if (jump75 && hp < stats.stats[StatType.HP].Value * 0.75f)
        {
            jump75 = false;
            ChangeState(StateOfEnemy.Attack);
            currentPatten = StartCoroutine(OnBackJump(true));
        }

        if (jump25 && hp < stats.stats[StatType.HP].Value * 0.25f)
        {
            jump25 = false;
            ChangeState(StateOfEnemy.Attack);
            currentPatten = StartCoroutine(OnBackJump(true));
        }

        if (!isBerserk && hp < stats.stats[StatType.HP].Value * 0.5)
        {
            isBerserk = true;
            ChangeState(StateOfEnemy.Attack);
            currentPatten = StartCoroutine(Roar());
        }
    }

    int hitCount = 0;

    public override void OnHit(float damage, float penetration)
    {
        base.OnHit(damage, penetration);

        hitCount++;
        if (hitCount >= 10)
        {
            isBackJump = true;
            hitCount = 0;
        }
    }

    [SerializeField, Header("로직 설정")]
    float fireballAttackRange;
    [SerializeField]
    float chackBackjumpRange;

    public bool ChackPatten()
    {
        if (currentPatten != null)
        {
            //Debug.Log("IsPatten");
            return false;
        }
        if (isPattern) return true;

        if (isBackJump)
        {
            currentPatten = StartCoroutine(OnBackJump(false));
            return false;
        }

        if (Vector3.Distance(Player.instance.transform.position, transform.position) < fireballAttackRange)
        {
            int temp = Random.Range(0, isBerserk ? 3 : 2);
            switch (temp)
            {
                case 0:
                    currentPatten = StartCoroutine(HandSlash());
                    return false;
                case 1:
                    currentPatten = StartCoroutine(HandDown());
                    return false;
                case 2:
                    currentPatten = StartCoroutine(Roar());
                    return false;
            }
        }

        if (isBerserk)
        {
            currentPatten = StartCoroutine(Fireball1());
            return false;
        }

        currentPatten = StartCoroutine(Fireball0());
        return false;

        //return true;
    }

    [SerializeField, Header("BackJump")]
    float backJumpRange;

    IEnumerator OnBackJump(bool fireball)
    {
        animator.SetTrigger("BackJump");

        OnAttackMove(30f, 0.05f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(39f / 60f));
        OnAttackMove(28f, -backJumpRange, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(43f / 60f));

        isBackJump = false;

        if (isBerserk || fireball)
        {
            currentPatten = StartCoroutine(Fireball0());
            yield break;
        }

        currentPatten = null;
        isPattern = true;
    }

    [SerializeField, Header("Fireball")]
    Transform fireballSpownPoint;
    [SerializeField]
    GameObject fireballFrepab;
    [SerializeField]
    float fireballSpeed;

    IEnumerator Fireball0()
    {
        EnemyRangeAttack fireball = Instantiate(fireballFrepab).GetComponent<EnemyRangeAttack>();
        fireball.SetEnemy(this);
        animator.SetTrigger("Fireball0");

        yield return StartCoroutine(WaitForSecondsOfPertten(35f / 60f));
        isLookAtPlayer = false;
        Vector3 dir = (Player.instance.transform.position - transform.position).normalized * fireballSpeed;
        yield return StartCoroutine(WaitForSecondsOfPertten(15f / 60f));
        //attack
        fireball.SetForce(dir, fireballSpownPoint, 20f / fireballSpeed);
        yield return StartCoroutine(WaitForSecondsOfPertten(39f / 60f));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    IEnumerator Fireball1()
    {
        EnemyRangeAttack fireball0 = Instantiate(fireballFrepab).GetComponent<EnemyRangeAttack>();
        EnemyRangeAttack fireball1 = Instantiate(fireballFrepab).GetComponent<EnemyRangeAttack>();
        fireball0.SetEnemy(this);
        fireball1.SetEnemy(this);
        animator.SetTrigger("Fireball1");

        yield return StartCoroutine(WaitForSecondsOfPertten(35f / 60f));
        isLookAtPlayer = false;
        Vector3 dir = (Player.instance.transform.position - transform.position).normalized * fireballSpeed;
        yield return StartCoroutine(WaitForSecondsOfPertten(15f / 60f));
        //attack0
        fireball0.SetForce(dir, fireballSpownPoint, 20f / fireballSpeed);
        yield return StartCoroutine(WaitForSecondsOfPertten(45f / 60f));
        LookAtPlayer();
        dir = (Player.instance.transform.position - transform.position).normalized * fireballSpeed;
        yield return StartCoroutine(WaitForSecondsOfPertten(15f / 60f));
        //attack1
        fireball1.SetForce(dir, fireballSpownPoint, 20f / fireballSpeed);
        yield return StartCoroutine(WaitForSecondsOfPertten(39f / 60f));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [SerializeField, Header("HandSlash")]
    Collider handSlashAttackCollider0;
    [SerializeField]
    Collider handSlashAttackCollider1;
    [SerializeField]
    float handSlashAttackRange;

    IEnumerator HandSlash()
    {
        animator.SetTrigger("HandSlash");
        StartCoroutine(ShotDesh(handSlashAttackRange));

        //move0
        OnAttackMove(29f, 0.05f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(34f / 60f));
        isLookAtPlayer = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(9f / 60f));
        //move1
        OnAttackMove(8f, 0.15f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        //attack0
        handSlashAttackCollider0.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        handSlashAttackCollider0.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(17f / 60f));
        //move2
        OnAttackMove(16f, 0.06f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(15f / 60f));
        //move3
        OnAttackMove(36f, 0.04f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(8f / 60f));
        //attack1
        handSlashAttackCollider1.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        handSlashAttackCollider1.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(40f / 60f));
        //move4
        OnAttackMove(38f, 0.13f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(38f / 60f));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [SerializeField, Header("HandDown")]
    Collider handDownAttackCollider;
    [SerializeField]
    float handDownRange;

    IEnumerator HandDown()
    {
        animator.SetTrigger("HandDown");
        StartCoroutine(ShotDesh(handDownRange));

        //move0
        OnAttackMove(51f, 0.22f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        circleDengger.Setup(handDownAttackCollider.transform.position + Vector3.down, 2f, 42f / 60f);
        yield return StartCoroutine(WaitForSecondsOfPertten(26f / 60f));
        isLookAtPlayer = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(15f / 60f));
        //attack
        handDownAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        handDownAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        //move1
        OnAttackMove(62f, 0.22f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(62f / 60f));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    IEnumerator Roar()
    {
        animator.SetTrigger("Roar");
        StartCoroutine(OnStendup());

        //move0
        OnAttackMove(29f, -0.27f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(28f / 60f));
        //move1
        OnAttackMove(16f, 0.07f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(16f / 60f));


        int temp = Random.Range(0, 2);
        if (temp == 0)
        {
            currentPatten = StartCoroutine(HandSmash());
            yield break;
        }
        currentPatten = StartCoroutine(HandSwipe());
    }

    [SerializeField, Header("HandSmash")]
    Collider HandSmashAttack0Collider;
    [SerializeField]
    Collider HandSmashAttack1Collider;

    IEnumerator HandSmash()
    {
        animator.SetTrigger("HandSmash");

        yield return StartCoroutine(WaitForSecondsOfPertten(18f / 60f));
        //move0
        OnAttackMove(26f, 0.12f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(15f / 60f));
        isLookAtPlayer = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(10f / 60f));
        //move1
        OnAttackMove(11f, 0.26f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        //attack0
        HandSmashAttack0Collider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        HandSmashAttack0Collider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        circleDengger.Setup(HandSmashAttack1Collider.transform.position + Vector3.down, 7, 32f / 60f);
        yield return StartCoroutine(WaitForSecondsOfPertten(28f / 60f));
        //move2
        OnAttackMove(16f, -0.23f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));
        //attack1
        HandSmashAttack1Collider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        HandSmashAttack1Collider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(39f / 60f));
        //move3
        StartCoroutine(OnStenddown());
        OnAttackMove(41f, 0.4f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(66f / 60f));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [SerializeField, Header("HandSwipe")]
    Collider HandSwipeAttackCollider;

    IEnumerator HandSwipe()
    {
        animator.SetTrigger("HandSwipe");

        //move0
        OnAttackMove(79f, 0.48f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        squareDengger.Setup(HandSwipeAttackCollider.transform.position + Vector3.down, 3f, 8f, 86f);
        yield return StartCoroutine(WaitForSecondsOfPertten(70f / 60f));
        isLookAtPlayer = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(3f / 60f));
        //move1
        OnAttackMove(7f, 0.04f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        //move2
        OnAttackMove(7f, 0.18f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        //attack
        HandSwipeAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(3f / 60f));
        //move3
        OnAttackMove(15f, -0.05f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(2f / 60f));
        HandSwipeAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(33f / 60f));
        //move4
        StartCoroutine(OnStenddown());
        OnAttackMove(56f, 0.5f * 4, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(106f / 60f));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    IEnumerator IsBackJump()
    {
        float playerDistance;
        float tempTime = 0;
        while (true)
        {
            playerDistance = Vector3.Distance(Player.instance.transform.position, transform.position);
            if (playerDistance < chackBackjumpRange)
            {
                tempTime += Time.deltaTime;
            }
            else
            {
                tempTime = 0;
            }

            if (tempTime >= 6)
            {
                isBackJump = true;
                tempTime = 0;
            }

            yield return null;
        }
    }

    IEnumerator ShotDesh(float distance)
    {
        float tempDistance = Vector3.Distance(Player.instance.transform.position, transform.position);

        while (tempDistance > distance)
        {
            movement.ToMove(Player.instance.transform.position - transform.position, shotDeshSpeed);

            yield return null;

            tempDistance = Vector3.Distance(Player.instance.transform.position, transform.position);
        }
        yield break;
    }

    [SerializeField, Header("버그 수정용")]
    GameObject randerObj;

    IEnumerator OnStendup()
    {
        //43f, 2.5f
        Vector3 tempPosition = randerObj.transform.localPosition;
        float timeTime = 0;
        OnAttackMove(43f, -3f, false);
        while (randerObj.transform.localPosition.z != 0)
        {
            Vector3 tempvec = Vector3.Lerp(tempPosition, Vector3.zero, timeTime / 0.71666f);
            randerObj.transform.localPosition = tempvec;

            timeTime += Time.deltaTime;

            yield return null;
        }

    }

    IEnumerator OnStenddown()
    {
        Vector3 tempPosition = randerObj.transform.localPosition;
        Vector3 tartgetvec = new Vector3 (0, 0, -3f);
        float timeTime = 0;
        OnAttackMove(43f, 3f, false);
        while (randerObj.transform.localPosition.z != -3f)
        {
            Vector3 tempvec = Vector3.Lerp(tempPosition, tartgetvec, timeTime / 0.71666f);
            randerObj.transform.localPosition = tempvec;

            timeTime += Time.deltaTime;

            yield return null;
        }
    }
}
