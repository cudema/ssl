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

    float startupTime = 1.0f;

    Boss0Patten lastUsedPatten = Boss0Patten.None;
    Coroutine currentPatten = null;

    void Start()
    {
        enemyStates[0] = new Wander(this, sensingRange, attackRange);
        enemyStates[1] = new Track(this, sensingRange, attackRange);
        enemyStates[2] = new Boss0Attack(this, sensingRange, attackRange);
    } 

    protected override void ChangedHP()
    {
        base.ChangedHP();

        if (!IsPatternLocked && hp < maxHP * 0.3)
        {
            IsPatternLocked = true;
            startupTime = 0.5f;
        }
    }

    [Header("BearSlash")]
    [SerializeField]
    float bearSlashStartingRange = 2.5f;
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

        yield return new WaitForSeconds(bearSlashStartupTime * startupTime);

        movement.LookAtTarget(Player.instance.transform.position);

        bearSlashAttackCollider.enabled = true;
        yield return new WaitForSeconds(bearSlashActiveTime);
        bearSlashAttackCollider.enabled = false;

        yield return new WaitForSeconds(bearSlashStartupTime * startupTime);

        movement.LookAtTarget(Player.instance.transform.position);

        bearSlashAttackCollider.enabled = true;
        yield return new WaitForSeconds(bearSlashActiveTime);
        bearSlashAttackCollider.enabled = false;
                
        yield return new WaitForSeconds(bearSlashStartupTime * startupTime);

        movement.LookAtTarget(Player.instance.transform.position);

        bearSlashAttackCollider.enabled = true;
        yield return new WaitForSeconds(bearSlashActiveTime);
        bearSlashAttackCollider.enabled = false;
        
        yield return new WaitForSeconds(bearSlashRecoveryTime);

        currentPatten = null;
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
        lastUsedPatten = Boss0Patten.GroundSmash;
     
        yield return new WaitForSeconds(groundSmashStartupTime * startupTime);

        groundSmashAttackCollider.enabled = true;
        yield return new WaitForSeconds(groundSmashActiveTime);
        groundSmashAttackCollider.enabled = false;

        yield return new WaitForSeconds(groundSmashRecoveryTime);

        currentPatten = null;
    }

    [Header("FinalStrike")]
    [SerializeField]
    float finalStrikeStartingRange = 7.0f;
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

        yield return new WaitForSeconds(finalStrikeStartupTime * startupTime);

        finalStrikeAttackCollider.enabled = true;
        yield return new WaitForSeconds(finalStrikeActiveTime);
        finalStrikeAttackCollider.enabled = false;

        yield return new WaitForSeconds(finalStrikeRecoveryTime);

        currentPatten = null;
    }

    [Header("PhantomCharge")]
    [SerializeField]
    float phantomChargeStartingRange = 8.0f;
    [SerializeField]
    Collider phantomChargeAttackCollider;
    [SerializeField]
    float phantomChargeStartupTime = 1.5f;
    [SerializeField]
    float phantomChargeActiveTime = 0.7f;
    [SerializeField]
    float phantomChargeRecoveryTime = 2.5f;
    [SerializeField]
    float phantomChargeCooldown = 15.0f;

    IEnumerator PhantomCharge()
    {
        lastUsedPatten = Boss0Patten.PhantomCharge;
        
        yield return null;

        currentPatten = null;
    }


    public bool ChackPatten()
    {
        if (currentPatten != null)
        {
            return false;
        }

        float distanceToPlayer = Vector3.Distance(Player.instance.transform.position, transform.position);

        // if (distanceToPlayer > 6)
        // {
        //      //영혼돌진 사용
        //      currentPatten = StartCoroutine(PhantomCharge());
        // }

        if (lastUsedPatten == Boss0Patten.FinalStrike || distanceToPlayer < bearSlashStartingRange)
        {
            //베어가르기 사용
            currentPatten = StartCoroutine(BearSlash());
            return false;
        }
        if (distanceToPlayer < groundSmashStartingRange && lastUsedPatten != Boss0Patten.GroundSmash)
        {
            //대지강타 사용
            currentPatten = StartCoroutine(GroundSmash());
            return false;
        }
        if (distanceToPlayer < finalStrikeStartingRange && lastUsedPatten != Boss0Patten.FinalStrike && lastUsedPatten != Boss0Patten.PhantomCharge)
        {
            //최후의 일격 사용
            currentPatten = StartCoroutine(FinalStrike());
            return false;
        }

        return true;
    }
}
