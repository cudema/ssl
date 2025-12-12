using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IHealthable
{
    [SerializeField]
    BoxCollider attackCollider;
    [SerializeField]
    GameObject hitEffect;

    PlayerWeapon playerWeapon;

    int switchingGauge;
    float damage;

    void Awake()
    {
        playerWeapon = GetComponent<PlayerWeapon>();
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
        attackCollider.size = weaponAttackData.AttackRange;
        attackCollider.center = new Vector3(0, 0, weaponAttackData.AttackRange.z / 2);
    }

    void OnTriggerEnter(Collider other)
    {
        IHealthable tmep = other.GetComponent<IHealthable>();
        if (tmep != null && !other.CompareTag("Player"))
        {
            tmep.OnHit(Player.instance.AttackDamage * damage, Player.instance.Penetration);
            Player.instance.SwitchingGauge += switchingGauge;
        }
    }

    public void OnHit(float damage, float penetration)
    {
        Player.instance.CurrentHp -= damage * (1 - (0.5f * (Player.instance.Defense * (1 - 0.5f * penetration / 100)) / 100));

        if (Player.instance.CurrentHp <= 0)
        {
            StageManager.instance.EndRun();
        }
    }
}
