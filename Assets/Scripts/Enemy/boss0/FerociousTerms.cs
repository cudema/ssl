using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Boss0Patten
{
    None = -1,
    BearSlash = 0,
    GroundSmash,
    FinalStrike,
    PhantomCharge,
    GroundBomb
}

public class FerociousTerms : EnemyBase
{
    [SerializeField]
    DenggerEffectBase circleDengger;    
    [SerializeField]
    DenggerEffectBase squareDengger;
    bool IsPatternLocked = false;

    Boss0Patten lastUsedPatten = Boss0Patten.None;
    Coroutine currentPatten = null;

    public bool isPattern = false;

    List<int> ranges = new List<int>();

    void Start()
    {
        enemyStates[2] = new Boss0Attack(this, sensingRange, attackRange);
        enemyStates[3] = new Alert(this, sensingRange, attackRange);
        currentState = enemyStates[0];
        phantomChargeChackCooldown = Time.time;
        hp = stats.stats[StatType.HP].Value;

        for (int i = 0; i < range.Length; i++)
        {
            for (int j = 0; j < range[i]; j++)
            {
                ranges.Add(i);
            }
        }
    } 

    protected override void ChangedHP()
    {
        base.ChangedHP();

        if (!IsPatternLocked && hp < stats.stats[StatType.HP].Value * 0.3)
        {
            IsPatternLocked = true;
            animator.speed = 2f;
            timeScale = 2f;
        }
    }

    [Header("BearSlash")]
    [SerializeField]
    float bearSlashStartingRange = 6.0f;
    [SerializeField]
    float bearSlashAttackRange = 2.5f;
    [SerializeField]
    Collider bearSlashAttackCollider;
    [SerializeField]
    float bearSlashRecoveryTime = 0.1f;

    IEnumerator BearSlash()
    {
        lastUsedPatten = Boss0Patten.BearSlash;
        Vector3 dir = Player.instance.transform.position - transform.position;

        while (dir.magnitude > bearSlashAttackRange)
        {
            animator.SetBool("isMove", true);
            dir = Player.instance.transform.position - transform.position;
            movement.ToMove(dir.normalized);

            yield return null;
        }
        
        animator.SetBool("isMove", false);
        animator.SetTrigger("BearSlash");
        isLookAtPlayer = false;
        
        yield return StartCoroutine(WaitForSecondsOfPertten(51f / 60f));

        bearSlashAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        bearSlashAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));

        isLookAtPlayer = true;

        yield return StartCoroutine(WaitForSecondsOfPertten(61f / 60f));

        isLookAtPlayer = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));

        bearSlashAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        bearSlashAttackCollider.enabled = false;
        
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));

        isLookAtPlayer = true;

        yield return StartCoroutine(WaitForSecondsOfPertten(54f / 60f));

        isLookAtPlayer = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));

        bearSlashAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        bearSlashAttackCollider.enabled = false;
        
        yield return StartCoroutine(WaitForSecondsOfPertten(65f / 60f));
        yield return StartCoroutine(WaitForSecondsOfPertten(bearSlashRecoveryTime));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [Header("GroundSmash")]
    [SerializeField]
    float groundSmashStartingRange = 4.0f;
    [SerializeField]
    Collider groundSmashAttackCollider;
    [SerializeField]
    float groundSmashRecoveryTime = 0.1f;

    IEnumerator GroundSmash()
    {
        animator.SetTrigger("GroundSmash");
        lastUsedPatten = Boss0Patten.GroundSmash;
     
        yield return StartCoroutine(WaitForSecondsOfPertten(15f / 60f));
        isLookAtPlayer = false;
        circleDengger.Setup(groundSmashAttackCollider.transform.position + Vector3.down, 1.8f, 50f / 60f);
        yield return StartCoroutine(WaitForSecondsOfPertten(50f / 60f));

        groundSmashAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        groundSmashAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(140f / 60f));
        yield return StartCoroutine(WaitForSecondsOfPertten(groundSmashRecoveryTime));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [Header("GroundBomb")]
    [SerializeField]
    float groundBombStartingRange = 4.0f;
    [SerializeField]
    Collider groundBombAttackCollider;
    [SerializeField]
    float groundBombRecoveryTime = 0.1f;

    IEnumerator GroundBomb()
    {
        animator.SetTrigger("GroundBomb");
        lastUsedPatten = Boss0Patten.GroundBomb;
     
        OnAttackMove(30f, -2.5f, false);

        yield return StartCoroutine(WaitForSecondsOfPertten(50f / 60f));
        isLookAtPlayer = false;
        circleDengger.Setup(groundBombAttackCollider.transform.position + Vector3.down, 2.5f, 85f / 60f);
        yield return StartCoroutine(WaitForSecondsOfPertten(85f / 60f));

        groundBombAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        groundBombAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(25f / 60f));

        circleDengger.Setup(groundBombAttackCollider.transform.position + Vector3.down, 2.5f, 59f / 60f);

        yield return StartCoroutine(WaitForSecondsOfPertten(59f / 60f));

        OnAttackMove(30f, 2f, false);

        groundBombAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        groundBombAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(51f / 60f));
        yield return StartCoroutine(WaitForSecondsOfPertten(groundBombRecoveryTime));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [Header("CrushCharge")]
    [SerializeField]
    float crushChargeStartingRange = 4.0f;
    [SerializeField]
    Collider crushChargeAttack0Collider;
    [SerializeField]
    Collider crushChargeAttack1Collider;
    [SerializeField]
    float crushChargeRecoveryTime = 0.1f;

    IEnumerator CrushCharge()
    {
        animator.SetTrigger("CrushCharge");
        lastUsedPatten = Boss0Patten.GroundBomb;
     
        isLookAtPlayer = false;
        OnAttackMove(30f, 5.8f, false);

        yield return StartCoroutine(WaitForSecondsOfPertten(32f / 60f));

        crushChargeAttack0Collider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(1f / 60f));
        OnAttackMove(30f, 1.02f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(6f / 60f));
        crushChargeAttack0Collider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        OnAttackMove(30f, 0.55f, false);
        yield return StartCoroutine(WaitForSecondsOfPertten(46f / 60f));

        OnAttackMove(30f, 5f, false);
        crushChargeAttack1Collider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(7f / 60f));
        crushChargeAttack1Collider.enabled = false;
        
        yield return StartCoroutine(WaitForSecondsOfPertten(82f / 60f));
        yield return StartCoroutine(WaitForSecondsOfPertten(crushChargeRecoveryTime));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [Header("FinalStrike")]
    [SerializeField]
    float finalStrikeStartingRange = 7.0f;
    [SerializeField]
    Collider finalStrikeAttackCollider;
    [SerializeField]
    float finalStrikeRecoveryTime = 3.0f;

    IEnumerator FinalStrike()
    {
        animator.SetTrigger("FinalStrike");
        lastUsedPatten = Boss0Patten.FinalStrike;
        isLookAtPlayer = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(98f / 60f));

        finalStrikeAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(5f / 60f));
        finalStrikeAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(148f / 60f));
        yield return StartCoroutine(WaitForSecondsOfPertten(finalStrikeRecoveryTime));

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [Header("PhantomCharge")]
    // [SerializeField]
    // float phantomChargeStartingRange = 8.0f;
    [SerializeField]
    Collider phantomChargeAttackCollider;
    [SerializeField]
    float phantomChargeStartupTime = 1.5f;
    // [SerializeField]
    // float phantomChargeActiveTime = 0.7f;
    [SerializeField]
    float phantomChargeRecoveryTime = 2.5f;
    [SerializeField]
    float phantomChargeCooldown = 15.0f;
    [SerializeField]
    float phantomChargeSpeed = 12f;
    [SerializeField]
    float phantomChargeRushTime = 1.5f;
    [SerializeField]
    GameObject agoPrefab;

    float phantomChargeChackCooldown;

    IEnumerator PhantomCharge()
    {
        lastUsedPatten = Boss0Patten.PhantomCharge;
        AlterEgo ego0 = Instantiate(agoPrefab).GetComponent<AlterEgo>();
        AlterEgo ego1 = Instantiate(agoPrefab).GetComponent<AlterEgo>();

        ego0.Setup(phantomChargeSpeed);
        ego1.Setup(phantomChargeSpeed);

        Vector3 target = transform.position - Player.instance.transform.position;
        target.y = 0;

        Vector3 temp = Quaternion.Euler(new Vector3(0, 30, 0)) * target;

        ego0.transform.position = Player.instance.transform.position + temp;

        temp = Quaternion.Euler(new Vector3(0, -30, 0)) * target;

        ego1.transform.position = Player.instance.transform.position + temp;

        ego0.transform.LookAt(Player.instance.transform.position);
        ego1.transform.LookAt(Player.instance.transform.position);
        movement.LookAtTarget(Player.instance.transform.position);

        yield return StartCoroutine(WaitForSecondsOfPertten(phantomChargeStartupTime));

        ego0.OnGo();
        ego1.OnGo();
        animator.SetTrigger("Ready");

        yield return StartCoroutine(WaitForSecondsOfPertten(1f));

        movement.Controller.enabled = false;
        isMove = false;
        float tempTime = 0;

        phantomChargeAttackCollider.enabled = true;
        while (phantomChargeRushTime > tempTime)
        {
            transform.position += -target.normalized * phantomChargeSpeed * Time.deltaTime;
            tempTime += Time.deltaTime;

            yield return null;
        }
        
        animator.SetTrigger("End");
        ego0.Stop();
        ego1.Stop();
        movement.Controller.enabled = true;
        isMove = true;
        phantomChargeAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(phantomChargeRecoveryTime / 2));

        ego0.OffRender();
        ego1.OffRender();

        yield return StartCoroutine(WaitForSecondsOfPertten(phantomChargeRecoveryTime / 2));

        target = transform.position - Player.instance.transform.position;
        target.y = 0;

        temp = Quaternion.Euler(new Vector3(0, 30, 0)) * target;

        ego0.transform.position = Player.instance.transform.position + temp;

        temp = Quaternion.Euler(new Vector3(0, -30, 0)) * target;

        ego1.transform.position = Player.instance.transform.position + temp;

        ego0.transform.LookAt(Player.instance.transform.position);
        ego1.transform.LookAt(Player.instance.transform.position);
        movement.LookAtTarget(Player.instance.transform.position);

        ego0.OnRender();
        ego1.OnRender();


        yield return StartCoroutine(WaitForSecondsOfPertten(phantomChargeStartupTime));

        ego0.OnGo();
        ego1.OnGo();
        animator.SetTrigger("Ready");

        yield return StartCoroutine(WaitForSecondsOfPertten(1f));

        movement.Controller.enabled = false;
        isMove = false;
        tempTime = 0;

        phantomChargeAttackCollider.enabled = true;
        while (phantomChargeRushTime > tempTime)
        {
            transform.position += -target.normalized * phantomChargeSpeed * Time.deltaTime;
            tempTime += Time.deltaTime;

            yield return null;
        }
        
        animator.SetTrigger("End");
        ego0.Stop();
        ego1.Stop();
        movement.Controller.enabled = true;
        isMove = true;
        phantomChargeAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(phantomChargeRecoveryTime / 2));

        ego0.OffRender();
        ego1.OffRender();

        yield return StartCoroutine(WaitForSecondsOfPertten(phantomChargeRecoveryTime / 2));

        target = transform.position - Player.instance.transform.position;
        target.y = 0;

        temp = Quaternion.Euler(new Vector3(0, 30, 0)) * target;

        ego0.transform.position = Player.instance.transform.position + temp;

        temp = Quaternion.Euler(new Vector3(0, -30, 0)) * target;

        ego1.transform.position = Player.instance.transform.position + temp;

        ego0.transform.LookAt(Player.instance.transform.position);
        ego1.transform.LookAt(Player.instance.transform.position);
        movement.LookAtTarget(Player.instance.transform.position);

        ego0.OnRender();
        ego1.OnRender();


        yield return StartCoroutine(WaitForSecondsOfPertten(phantomChargeStartupTime));

        ego0.OnGo();
        ego1.OnGo();
        animator.SetTrigger("Ready");

        yield return StartCoroutine(WaitForSecondsOfPertten(1f));

        movement.Controller.enabled = false;
        isMove = false;
        tempTime = 0;

        phantomChargeAttackCollider.enabled = true;
        while (phantomChargeRushTime > tempTime)
        {
            transform.position += -target.normalized * phantomChargeSpeed * Time.deltaTime;
            tempTime += Time.deltaTime;

            yield return null;
        }
        
        animator.SetTrigger("End");
        ego0.Stop();
        ego1.Stop();
        movement.Controller.enabled = true;
        isMove = true;
        phantomChargeAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(phantomChargeRecoveryTime / 2));

        ego0.OffRender();
        ego1.OffRender();

        yield return StartCoroutine(WaitForSecondsOfPertten(phantomChargeRecoveryTime / 2));

        Destroy(ego0);
        Destroy(ego1);

        yield return StartCoroutine(WaitForSecondsOfPertten(phantomChargeRecoveryTime));

        phantomChargeChackCooldown = Time.time;

        isLookAtPlayer = true;
        currentPatten = null;
        isPattern = true;
    }

    [SerializeField]
    int[] range;

    public bool ChackPatten()
    {
        if (currentPatten != null)
        {
            //Debug.Log("IsPatten");
            return false;
        }
        if (isPattern) return true;

        float distanceToPlayer = Vector3.Distance(Player.instance.transform.position, transform.position);

        if (lastUsedPatten == Boss0Patten.FinalStrike)
        {
            //베어가르기 사용
            currentPatten = StartCoroutine(BearSlash());
            StopMoveAnimation();
            return false;
        }
        
        if (distanceToPlayer > 6 && phantomChargeCooldown < Time.time - phantomChargeChackCooldown)
        {
             //영혼돌진 사용
             currentPatten = StartCoroutine(PhantomCharge());
             StopMoveAnimation();
             return false;
        }
        int temp = Random.Range(0, ranges.Count);

        switch (ranges[temp])
        {
            case 0:
                currentPatten = StartCoroutine(GroundSmash());
                break;
            case 1:
                currentPatten = StartCoroutine(GroundBomb());
                break;
            case 2:
                currentPatten = StartCoroutine(CrushCharge());
                break;
            case 3:
                currentPatten = StartCoroutine(FinalStrike());
                break;
            case 4:
                currentPatten = StartCoroutine(BearSlash());
                break;
        }
    
        StopMoveAnimation();
        return false;
    }
}
