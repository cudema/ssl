using UnityEngine;

public class Weapon
{
    protected float weaponDamege;
    protected GameObject weaponPrefab;

    protected float attackDilay = 0;
    float tempTime;

    public virtual void EquipWeapon()
    {
        Debug.Log("NoChangeWeaponSkill");
    }

    public void AttackWeapon()
    {
        if (Time.time - tempTime > attackDilay)
        {
            tempTime = Time.time;

            OnAttack();
        }
    }

    public virtual void UnequipWeapon()
    {
        Debug.Log("NoChangeWeaponSkill");
    }

    protected virtual void OnAttack()
    {

    }
}
