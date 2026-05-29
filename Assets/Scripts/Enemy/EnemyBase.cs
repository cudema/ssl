using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public abstract class EnemyBase : MonoBehaviour, IHealthable
{
    [SerializeField]
    public float hp;
    [SerializeField]
    protected float rotateSpeed;
    [SerializeField]
    protected float sensingRange;
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    public float alertTime;
    [SerializeField]
    public float alertSpeed;
    [SerializeField]
    ParticleSystem hitEffect;

    EnenyHPBar enenyHPBar;

    [HideInInspector]
    public PlayerStats stats;

    protected bool isMove = true;

    protected EnemyState currentState;
    protected EnemyState[] enemyStates = new EnemyState[5];

    [HideInInspector]
    public Movement movement;

    StageManager stagemanager;

    [SerializeField, Header("사운드")]
    SoundSetting hitSound;

    [SerializeField]
    public Animator animator;
    protected bool isAttacking = false;
    [HideInInspector]
    public bool isKnockback = false;

    bool IsImmune = false;

    bool isHitable = false;

    protected float timeScale = 1.0f;

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        movement = GetComponent<Movement>();
        enenyHPBar = GetComponent<EnenyHPBar>();

        enemyStates[0] = new Wander(this, sensingRange, attackRange);
        enemyStates[1] = new Track(this, sensingRange, attackRange);
        enemyStates[4] = new Dead(this, sensingRange, attackRange);
    }

    void Start()
    {
        enemyStates[2] = new Attack(this, sensingRange, attackRange);
        enemyStates[3] = new Alert(this, sensingRange, attackRange);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
    void Update()
    {
        if (isMove)
        {
            movement.OnGravity();
        }
        //Debug.Log(currentState);
    }

    public void ChangeState(StateOfEnemy newState)
    {
        //Debug.Log($"{currentState} {enemyStates[(int)newState]}");

        currentState.Escape();
        currentState = enemyStates[(int)newState];
        currentState.Start();
    }

    public virtual void Setup(StageManager stagemanager)
    {
        this.stagemanager = stagemanager;

        currentState = enemyStates[0];
        animator.Rebind();
        currentState.Start();
        hp = stats.stats[StatType.HP].Value;
        movement.SetSpeed(stats.stats[StatType.Speed].Value, rotateSpeed);
        StartCoroutine(Look());
        isHitable = true;
    }

    protected virtual void OnDead()
    {
        StopAllCoroutines();
        StartCoroutine(DeadDilay());
    }

    IEnumerator DeadDilay()
    {
        ChangeState(StateOfEnemy.Dead);
        isHitable = false;

        int temp = Random.Range(0, 2);

        if (temp == 0)
        {
            animator.SetTrigger("Dead0");
        }
        else
        {
            animator.SetTrigger("Dead1");
        }

        yield return new WaitForSeconds(1.5f);

        GetComponent<DeathVFXController>().PlayDeathVFX();

        yield return new WaitForSeconds(0.5f);

        stagemanager.AddCountDeadEnemy(this.gameObject);
    }

    public virtual void OnHit(float damage, float penetration)
    {
        if (!isHitable) return;
        hp -= damage * (1.0f - (0.5f * (stats.stats[StatType.Defence].Value * (1.0f - 0.5f * penetration / 100)) / 100));

        ChangedHP();
        SoundManager.instance.PlaySFX(hitSound);

        if (hp <= 0)
        {
            OnDead();
        }
    }

    public void OnPlayHitPaticl(Vector3 vector)
    {
        hitEffect.transform.localRotation = Quaternion.LookRotation(vector);
        hitEffect.Play();
    }

    protected virtual void ChangedHP()
    {
        enenyHPBar.UpdateHPBar();
    }

    public void OnStiffen(AttackStaggerTier staggerTier)
    {
        if (IsImmune) return;

        switch (staggerTier)
        {
            case AttackStaggerTier.None:
                return;
            case AttackStaggerTier.Light:
                //약경직 실행 코드
                break;
            case AttackStaggerTier.Heavy:
                //강경직 실행 코드
                break;
            default:
                Debug.LogError("Null Of StaggerTier With Enemy");
                return;
        }
    }

    public void OnTureDamage(float damage)
    {
        if (!movement.Controller.enabled) return;
        hp -= damage;
        
        ChangedHP();

        if (hp <= 0)
        {
            OnDead();
        }
    }

    Coroutine stiffening;
    bool isTimeTrue = true;

    public void OnAttackStiffen(WeaponAttackData data)
    {
        if (stiffening != null) StopCoroutine(stiffening);
        if (!isAttacking) 
        {
            animator.SetTrigger("Stiffen");
            stiffening = StartCoroutine(Knockback(data.KnockbackRange));
            return;
        }
        stiffening = StartCoroutine(AttackStiffen(data.StiffenTime));
    }

    public void OnAttackStiffen(float time)
    {
        if (stiffening != null) StopCoroutine(stiffening);
        if (!isAttacking) 
        {
            animator.SetTrigger("Stiffen");
            return;
        }
        stiffening = StartCoroutine(AttackStiffen(time));
    }

    IEnumerator AttackStiffen(float stiffen)
    {
        animator.speed = 0f;
        movement.SetSpeed(0);
        float tempSpeed = alertSpeed;
        alertSpeed = 0;
        isTimeTrue = false;

        yield return new WaitForSeconds(stiffen);

        animator.speed = 1f;
        movement.SetSpeed(stats.stats[StatType.Speed].Value);
        alertSpeed = tempSpeed;
        isTimeTrue = true;
    }

    [SerializeField, Range(0, 1)]
    float knockbackRange = 1;

    IEnumerator Knockback(float range)
    {
        isKnockback = true;
        Vector3 vector = (transform.position - Player.instance.transform.position).normalized;
        float tempTime = 0;
        while (tempTime < 0.1f)
        {
            movement.ToMove(vector * knockbackRange, range / 0.1f);
            tempTime += Time.deltaTime;
            yield return null;
        }

        isKnockback = false;
    }

    protected IEnumerator WaitForSecondsOfPertten(float second)
    {
        float timer = 0;

        while (timer <= second)
        {
            if (isTimeTrue)
            {
                timer += Time.deltaTime * timeScale;
            }

            yield return null;
        }
    }

    public virtual void OnAttack()
    {
        isAttacking = true;
        LookAtPlayer();
    }

    protected void LookAtPlayer()
    {
        movement.LookAtTarget(Player.instance.transform.position);
    }

    protected void LookAtPlayer(float speedPer)
    {
        movement.LookAtTarget(Player.instance.transform.position, speedPer);
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

    public float GetAttackTime()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public bool GetAttacking()
    {
        return isAttacking;
    }

    public float GetHpPer()
    {
        return hp / stats.stats[StatType.HP].Value;
    }

    public void OnAlertAnimator()
    {
        animator.SetBool("IsAlert", true);
    }

    public void OffAlertAnimator()
    {
        animator.SetBool("IsAlert", false);
    }

    Coroutine moveCoroutine;

    public void OnAttackMove(float actionTime, float actionDistance, bool lookAt)
    {
        // 기존 이동이 있다면 중지
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(ProcessAttackMove(actionTime / 60f, actionDistance, lookAt));
    }

    public void StopAttackMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
    }

    private IEnumerator ProcessAttackMove(float actionTime, float actionDistance, bool lookAt)
    {
        float elapsed = 0f;
        Vector3 direction = movement.renderTransform.forward;
        
        // 2. 이동 실행 (Action_Time 동안 진행)
        // 기획서대로 순간이동이 아닌 Velocity 기반의 부드러운 이동
        float speed = actionDistance / actionTime;

        while (elapsed < actionTime)
        {
            if (lookAt)
            {
                LookAtPlayer();
                direction = movement.renderTransform.forward;
            }

            // 물리 엔진에 의해 막히는 것은 Rigidbody가 알아서 처리함
            Vector3 moveAmount = direction * speed * Time.deltaTime;
            movement.Controller.Move(moveAmount);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    [HideInInspector]
    public bool isLookAtPlayer = true;

    IEnumerator Look()
    {
        while (true)
        {
            if (!isLookAtPlayer)
            {
                yield return null;
                continue;
            }
            LookAtPlayer();
            yield return null;
        }
    }
}
