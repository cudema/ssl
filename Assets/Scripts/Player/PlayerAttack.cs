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

    Transform hitEffect;

    int switchingGauge;
    float damage;

    void Awake()
    {
        playerWeapon = GetComponent<PlayerWeapon>();
        hitEffect = Instantiate(hitEffectPrefab).GetComponent<Transform>();
        DontDestroyOnLoad(hitEffect);
        effect = hitEffect.GetComponent<ParticleSystem>();
    }

    public void OnAttack()
    {
        attackCollider.enabled = true;
    }

    public void OffAttack()
    {
        attackCollider.enabled = false;
    }

    public void SetupAttackData(WeaponAttackData weaponAttackData)
    {
        switchingGauge = weaponAttackData.SwitchingGauge;
        damage = weaponAttackData.Damage;
        //attackCollider.size = weaponAttackData.AttackRange;
        //attackCollider.center = new Vector3(0, 0, weaponAttackData.AttackRange.z / 2);
    }

    void OnTriggerEnter(Collider other)
    {
        IHealthable tmep = other.GetComponent<IHealthable>();
        if (tmep != null)
        {
            tmep.OnHit(Player.instance.AttackDamage * damage, Player.instance.Penetration);
            Player.instance.SwitchingGauge += switchingGauge;
            hitEffect.position = other.transform.position;
            effect.Play();
            StartCoroutine(AttackStiffen());
        }
    }

    [SerializeField]
    Renderer stiffenObject;
    Coroutine stiffenCoroutine;

    public void OnStiffen(AttackStaggerTier staggerTier)
    {
        if (Player.instance.isInvincible) return;
        if (stiffenCoroutine != null) StopCoroutine(stiffenCoroutine);

        switch (staggerTier)
        {
            case AttackStaggerTier.None:
                return;
            case AttackStaggerTier.Light:
                //약경직 실행 코드
                stiffenObject.material.color = Color.yellow;
                stiffenObject.enabled = true;
                Player.instance.ImpossPlayerMove();
                playerWeapon.animator.SetBool("IsInputEnabled", false);
                stiffenCoroutine = StartCoroutine(stiffenTimer());
                return;
            case AttackStaggerTier.Heavy:
                //강경직 실행 코드
                stiffenObject.material.color = Color.blue;
                stiffenObject.enabled = true;
                Player.instance.ImpossPlayerMove();
                playerWeapon.animator.SetBool("IsInputEnabled", false);
                stiffenCoroutine = StartCoroutine(stiffenTimer());
                return;
            default:
                Debug.LogError("Null Of StaggerTier with Player");
                return;
        }
    }

    IEnumerator stiffenTimer()
    {
        yield return new WaitForSeconds(1f);

        stiffenObject.enabled = false;
        Player.instance.PossPlayerMove();
        playerWeapon.animator.SetBool("IsInputEnabled", true);
    }

    public void OnHit(float damage, float penetration)
    {
        if (Player.instance.isInvincible) return;

        Player.instance.CurrentHp -= damage * (1 - (0.5f * (Player.instance.Defense * (1 - 0.5f * penetration / 100)) / 100));

        if (Player.instance.CurrentHp <= 0)
        {
            Player.instance.OffPlayer();
            Destroy(Instantiate(dieEffect, transform), 3f);
            StageManager.instance.EndRun();
        }
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
}
