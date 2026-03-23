using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IHealthable
{
    Collider attackCollider;
    [SerializeField]
    GameObject hitEffectPrefab;
    [SerializeField]
    GameObject dieEffect;
    [SerializeField]
    float stiffen;

    PlayerWeapon playerWeapon;

    ParticleSystem effect;

    PlayerStats playerStats;

    PlayerEffectHandler playerEffectHandler;

    Transform hitEffect;

    private WeaponEffect weaponEffect;

    int switchingGauge;
    float damage;

    void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerWeapon = GetComponent<PlayerWeapon>();
        playerEffectHandler = GetComponent<PlayerEffectHandler>();

        hitEffect = Instantiate(hitEffectPrefab).GetComponent<Transform>();
        
        DontDestroyOnLoad(hitEffect);
        
        effect = hitEffect.GetComponent<ParticleSystem>();

        weaponEffect = playerWeapon.GetComponentInChildren<WeaponEffect>(true);
    }

    public void OnAttack()
    {
        if (attackCollider != null)
            attackCollider.enabled = true;

        WeaponEffect currentWeaponEffect = playerWeapon.GetComponentInChildren<WeaponEffect>(true);

        if (currentWeaponEffect != null)
            currentWeaponEffect.PlayTrail();
        else
            Debug.LogWarning("현재 무기의 WeaponEffect를 찾지 못함");
    }

    public void OffAttack()
    {
        if (attackCollider != null)
            attackCollider.enabled = false;

        WeaponEffect currentWeaponEffect = playerWeapon.GetComponentInChildren<WeaponEffect>(true);

        if (currentWeaponEffect != null)
            currentWeaponEffect.StopTrail();
    }

    public void SetupAttackData(WeaponAttackData weaponAttackData)
    {
        switchingGauge = weaponAttackData.SwitchingGauge;
        damage = weaponAttackData.Damage;
        Player.instance.SetStat(StatType.AttackDamage);
        //attackCollider.size = weaponAttackData.AttackRange;
        //attackCollider.center = new Vector3(0, 0, weaponAttackData.AttackRange.z / 2);
    }

    void OnTriggerEnter(Collider other)
    {
        IHealthable tmep = other.GetComponent<IHealthable>();
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (tmep != null)
        {
            float effectAddDamage = playerEffectHandler.OnAddDamage(enemy);
            float effectAddDamagePer = playerEffectHandler.OnAddDamagePer(enemy);

            tmep.OnHit(Player.instance.AttackDamage * damage * (effectAddDamagePer + 1.0f) + effectAddDamage , playerStats.stats[StatType.Penetration].Value);
            hitEffect.position = other.transform.position;
            //Debug.Log(other.transform.position);
            effect.Play();

            playerEffectHandler.OnCharacterAttack(enemy);

            StartCoroutine(AttackStiffen());

            if (Player.instance.isBattleAcceleration)
            {
                Player.instance.SwitchingGauge += switchingGauge;
            }
        }
    }

    [SerializeField]
    Renderer stiffenObject;
    Coroutine stiffenCoroutine;
    AttackStaggerTier currentTier;

    public void OnStiffen(AttackStaggerTier staggerTier)
    {
        if (Player.instance.isInvincible) return;
        if (Player.instance.IsImmune) return;
        if (currentTier >= staggerTier) return;
        if (stiffenCoroutine != null) StopCoroutine(stiffenCoroutine);

        currentTier = staggerTier;

        switch (staggerTier)
        {
            case AttackStaggerTier.None:
                return;
            case AttackStaggerTier.Light:
                //약경직 실행 코드
                Player.instance.IsInputEnabled = false;
                stiffenObject.enabled = true;
                Player.instance.ImpossPlayerMove();
                stiffenObject.material.color = Color.yellow;
                stiffenCoroutine = StartCoroutine(stiffenTimer(0.5f));
                return;
            case AttackStaggerTier.Heavy:
                //강경직 실행 코드
                Player.instance.IsInputEnabled = false;
                stiffenObject.enabled = true;
                Player.instance.ImpossPlayerMove();
                stiffenObject.material.color = Color.blue;
                stiffenCoroutine = StartCoroutine(stiffenTimer(1f));
                return;
            default:
                Debug.LogError("Null Of StaggerTier with Player");
                return;
        }
    }

    public void OnTureDamage(float damage)
    {
        Player.instance.CurrentHp -= damage;
        
        ChackHP();
    }

    IEnumerator stiffenTimer(float time)
    {
        yield return new WaitForSeconds(time);

        stiffenObject.enabled = false;
        Player.instance.PossPlayerMove();
        Player.instance.IsInputEnabled = true;
        currentTier = AttackStaggerTier.None;
    }

    public void OnHit(float damage, float penetration)
    {
        if (Player.instance.isInvincible) return;

        float effectAddDefence = playerEffectHandler.OnAddDefance();
        float effectAddDefencePer = playerEffectHandler.OnAddDefancePer();

        Player.instance.CurrentHp -= damage * (1 - (0.5f * ((playerStats.stats[StatType.Defence].Value + effectAddDefence) * (1 - 0.5f * penetration / 100)) / 100)) * (1 - effectAddDefencePer);

        ChackHP();
    }

    public void SetAttackCollider(Collider newCollider)
    {
        attackCollider = newCollider;
    }

    IEnumerator AttackStiffen()
    {
        playerWeapon.animator.speed = 0f;
        yield return new WaitForSeconds(stiffen);
        playerWeapon.animator.speed = 1;
    }

    void ChackHP()
    {
        if (Player.instance.CurrentHp <= 0)
        {
            Player.instance.OffPlayer();
            Destroy(Instantiate(dieEffect, transform), 3f);
            StageManager.instance.EndRun();
        }
    }
}
