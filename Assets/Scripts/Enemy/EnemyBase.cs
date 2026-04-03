using UnityEngine;

[RequireComponent(typeof(Movement))]
public abstract class EnemyBase : MonoBehaviour, IHealthable
{
    [SerializeField]
    protected float maxHP;
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
    [SerializeField]
    public float alertTime;
    [SerializeField]
    public float alertSpeed;
    [SerializeField]
    ParticleSystem hitEffect;

    protected bool isMove = true;

    protected EnemyState currentState;
    protected EnemyState[] enemyStates = new EnemyState[4];

    [HideInInspector]
    public Movement movement;

    StageManager stagemanager;

    [SerializeField]
    public Animator animator;
    protected bool isAttacking = false;

    bool IsImmune = false;

    public float AttackDamage
    {
        get => attackDamage;
    }

    void Awake()
    {
        movement = GetComponent<Movement>();
        movement.SetSpeed(speed, 10f);

        enemyStates[0] = new Wander(this, sensingRange, attackRange);
        enemyStates[1] = new Track(this, sensingRange, attackRange);
        
    }

    void Start()
    {
        enemyStates[2] = new Attack(this, sensingRange, attackRange);
        enemyStates[3] = new Alert(this, sensingRange, attackRange);
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

    public void Setup(StageManager stagemanager)
    {
        this.stagemanager = stagemanager;

        currentState = enemyStates[0];
        currentState.Start();
        hp = maxHP;
    }

    void OnDead()
    {
        stagemanager.AddCountDeadEnemy(this.gameObject);
    }

    public void OnHit(float damage, float penetration)
    {
        hp -= damage * (1 - (0.5f * (defense * (1 - 0.5f * penetration / 100)) / 100));

        ChangedHP();

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
        hp -= damage;

        if (hp <= 0)
        {
            OnDead();
        }
    }

    public virtual void OnAttack()
    {
        isAttacking = true;
        movement.LookAtTarget(Player.instance.transform.position);
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
        return hp / maxHP;
    }

    public void OnAlertAnimator()
    {
        animator.SetBool("IsAlert", true);
    }

    public void OffAlertAnimator()
    {
        animator.SetBool("IsAlert", false);
    }
}
