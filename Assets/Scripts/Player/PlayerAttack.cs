using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    BoxCollider attackCollider;

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
        damage = weaponAttackData.Datage;
        attackCollider.size = weaponAttackData.AttackRange;
        attackCollider.center = new Vector3(0, 0, weaponAttackData.AttackRange.z / 2);
    }

    void OnTriggerEnter(Collider other)
    {
        IHealthable tmep = other.GetComponent<IHealthable>();
        if (tmep != null)
        {
            tmep.OnHit(damage);
            playerWeapon.SwitchingGauge += switchingGauge;
        }
    }
}
