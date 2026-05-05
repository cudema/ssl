using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IHealthable
{
    Collider attackCollider;
    // [SerializeField]
    // GameObject hitEffectPrefab;
    [SerializeField]
    GameObject dieEffect;

    [HideInInspector]
    public WeaponAttackData currentAttackData;

    PlayerWeapon playerWeapon;

    //ParticleSystem effect;

    PlayerStats playerStats;

    PlayerEffectHandler playerEffectHandler;

    //Transform hitEffect;

    private WeaponEffect weaponEffect;

    HashSet<IHealthable> hitObj = new HashSet<IHealthable>();

    void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerWeapon = GetComponent<PlayerWeapon>();
        playerEffectHandler = GetComponent<PlayerEffectHandler>();

        //hitEffect = Instantiate(hitEffectPrefab).GetComponent<Transform>();
        
        //DontDestroyOnLoad(hitEffect);
        
        //effect = hitEffect.GetComponent<ParticleSystem>();

        weaponEffect = playerWeapon.GetComponentInChildren<WeaponEffect>(true);
    }

    public void OnAttack(int index)
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
            playerWeapon.currentWeapon.SetAttackIndex(index);
        }

        WeaponEffect currentWeaponEffect = attackCollider.GetComponent<WeaponEffect>();

        if (currentWeaponEffect != null)
            currentWeaponEffect.PlayTrail();
        else
            Debug.LogWarning("현재 무기의 WeaponEffect를 찾지 못함");
    }

    public void OffAttack()
    {
        if (attackCollider != null)
            attackCollider.enabled = false;

        WeaponEffect currentWeaponEffect = attackCollider.GetComponent<WeaponEffect>();

        if (currentWeaponEffect != null)
            currentWeaponEffect.StopTrail();

        hitObj.Clear();
    }

    public void SetupAttackData(WeaponAttackData weaponAttackData)
    {
        currentAttackData = weaponAttackData;
        Player.instance.SetStat(StatType.AttackDamage);
        //Debug.Log(damage);
        //attackCollider.size = weaponAttackData.AttackRange;
        //attackCollider.center = new Vector3(0, 0, weaponAttackData.AttackRange.z / 2);
    }

    void OnTriggerEnter(Collider other)
    {
        IHealthable tmep = other.GetComponent<IHealthable>();
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (tmep != null && hitObj.Add(tmep))
        {
            float effectAddDamage = playerEffectHandler.OnAddDamage(enemy);

            tmep.OnHit(Player.instance.AttackDamage * currentAttackData.Damage * (1.0f + Player.instance.playerStats.stats[StatType.IncreasedDamage].Value + effectAddDamage) , playerStats.stats[StatType.Penetration].Value);
            enemy.OnPlayHitPaticl(playerWeapon.GetWeaponRotate());
            //hitEffect.position = other.transform.position;
            //Debug.Log(other.transform.position);
            //effect.Play();

            playerEffectHandler.OnCharacterAttack(enemy);

            StartCoroutine(AttackStiffen());
            enemy.OnAttackStiffen(currentAttackData);


            Player.instance.SwitchingGauge += currentAttackData.SwitchingGauge;
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
                //stiffenObject.enabled = true;
                Player.instance.ImpossPlayerMove();
                playerWeapon.animator.SetTrigger("Stumble");
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
        if (Player.instance.perfectAvoid)
        {

            return;
        }
        if (Player.instance.isInvincible)
        {
            
            return;
        }

        Player.instance.CurrentHp -= damage 
        * (1 - (0.5f * (playerStats.stats[StatType.Defence].Value * (1 - 0.5f * penetration / 100)) / 100)) 
        * (1.0f - playerStats.stats[StatType.PerDefence].Value);

        ChackHP();
    }

    public void SetAttackCollider(Collider newCollider)
    {
        attackCollider = newCollider;
    }

    IEnumerator AttackStiffen()
    {
        playerWeapon.animator.speed = 0f;
        playerWeapon.StopParticle();

        yield return new WaitForSeconds(currentAttackData.StiffenTime);

        playerWeapon.animator.speed = 1;
        playerWeapon.PlayParticle();
    }

    void ChackHP()
    {
        if (Player.instance.CurrentHp <= 0)
        {
            Player.instance.ImpossPlayerMove();
            playerWeapon.animator.SetTrigger("Dead");
            Destroy(Instantiate(dieEffect, transform), 3f);
            StageManager.instance.EndRun();
        }
    }
}
