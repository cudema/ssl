using UnityEngine;

public class NoWeapon : Weapon
{
    protected override void OnAttack()
    {
        Debug.Log("Attack");
    }
}
