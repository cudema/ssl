using UnityEngine;

public class NoWeapon : Weapon
{
    public NoWeapon(float attackDilay)
    {
        this.attackDilay = attackDilay;
    }

    public override void EquipWeapon()
    {
        Debug.Log("NoWeponSkill");
    }

    protected override void OnAttack()
    {
        Debug.Log("Attack");
    }

    public override void UnequipWeapon()
    {
        Debug.Log("NoWeponSkill");
    }
}
