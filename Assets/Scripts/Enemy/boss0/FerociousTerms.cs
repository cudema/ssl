using System.Collections;
using UnityEngine;

public enum Boss0Patten
{
    None = -1,
    BearSlash = 0,
    GroundSmash,
    FinalStrike,
    PhantomCharge
}

public class FerociousTerms : EnemyBase
{
    bool IsPatternLocked = false;

    Boss0Patten lastUsedPatten = Boss0Patten.None;
    Coroutine currentPatten = null;

    public bool isPattern = false;

    void Start()
    {
        enemyStates[2] = new Boss0Attack(this, sensingRange, attackRange);
        enemyStates[3] = new Alert(this, sensingRange, attackRange);
        currentState = enemyStates[0];
        phantomChargeChackCooldown = Time.time;
        hp = maxHP;
    } 

    protected override void ChangedHP()
    {
        base.ChangedHP();

        if (!IsPatternLocked && hp < maxHP * 0.3)
        {
            IsPatternLocked = true;
            timeScale = 0.5f;
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
    float bearSlashStartupTime = 2.0f;
    [SerializeField]
    float bearSlashActiveTime = 0.4f;
    [SerializeField]
    float bearSlashRecoveryTime = 1.2f;

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
        movement.LookAtTarget(Player.instance.transform.position);
        
        yield return StartCoroutine(WaitForSecondsOfPertten(bearSlashStartupTime));

        bearSlashAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(bearSlashActiveTime));
        bearSlashAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(70f / 60f));

        movement.LookAtTarget(Player.instance.transform.position);

        bearSlashAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(bearSlashActiveTime));
        bearSlashAttackCollider.enabled = false;
        yield return StartCoroutine(WaitForSecondsOfPertten(64f / 60f));

        movement.LookAtTarget(Player.instance.transform.position);

        bearSlashAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(bearSlashActiveTime));
        bearSlashAttackCollider.enabled = false;
        
        yield return StartCoroutine(WaitForSecondsOfPertten(bearSlashRecoveryTime));

        currentPatten = null;
        isPattern = true;
    }

    [Header("GroundSmash")]
    [SerializeField]
    float groundSmashStartingRange = 4.0f;
    [SerializeField]
    Collider groundSmashAttackCollider;
    [SerializeField]
    float groundSmashStartupTime = 2.5f;
    [SerializeField]
    float groundSmashActiveTime = 0.5f;
    [SerializeField]
    float groundSmashRecoveryTime = 2.0f;

    IEnumerator GroundSmash()
    {
        animator.SetTrigger("GroundSmash");
        lastUsedPatten = Boss0Patten.GroundSmash;
     
        yield return StartCoroutine(WaitForSecondsOfPertten(groundSmashStartupTime));

        groundSmashAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(groundSmashActiveTime));
        groundSmashAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(groundSmashRecoveryTime));

        currentPatten = null;
        isPattern = true;
    }

    [Header("FinalStrike")]
    //[SerializeField]
    //float finalStrikeStartingRange = 7.0f;
    [SerializeField]
    Collider finalStrikeAttackCollider;
    [SerializeField]
    float finalStrikeStartupTime = 5.0f;
    [SerializeField]
    float finalStrikeActiveTime = 0.7f;
    [SerializeField]
    float finalStrikeRecoveryTime = 3.0f;

    IEnumerator FinalStrike()
    {
        lastUsedPatten = Boss0Patten.FinalStrike;

        yield return StartCoroutine(WaitForSecondsOfPertten(finalStrikeStartupTime));

        finalStrikeAttackCollider.enabled = true;
        yield return StartCoroutine(WaitForSecondsOfPertten(finalStrikeActiveTime));
        finalStrikeAttackCollider.enabled = false;

        yield return StartCoroutine(WaitForSecondsOfPertten(finalStrikeRecoveryTime));

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

        currentPatten = null;
        isPattern = true;
    }

    public bool ChackPatten()
    {
        if (currentPatten != null)
        {
            //Debug.Log("IsPatten");
            return false;
        }
        if (isPattern) return true;

        float distanceToPlayer = Vector3.Distance(Player.instance.transform.position, transform.position);

        if (distanceToPlayer > 6 && phantomChargeCooldown < Time.time - phantomChargeChackCooldown)
        {
             //영혼돌진 사용
             currentPatten = StartCoroutine(PhantomCharge());
             StopMoveAnimation();
             return false;
        }

        if (distanceToPlayer < groundSmashStartingRange && lastUsedPatten != Boss0Patten.GroundSmash)
        {
            //대지강타 사용
            currentPatten = StartCoroutine(GroundSmash());
            StopMoveAnimation();
            return false;
        }
        if (lastUsedPatten == Boss0Patten.FinalStrike || distanceToPlayer < bearSlashStartingRange)
        {
            //베어가르기 사용
            currentPatten = StartCoroutine(BearSlash());
            StopMoveAnimation();
            return false;
        }
        // if (distanceToPlayer < finalStrikeStartingRange && lastUsedPatten != Boss0Patten.FinalStrike && lastUsedPatten != Boss0Patten.PhantomCharge)
        // {
        //     //최후의 일격 사용
        //     currentPatten = StartCoroutine(FinalStrike());
        //     StopMoveAnimation();
        //     return false;
        // }

        return true;
    }
}
