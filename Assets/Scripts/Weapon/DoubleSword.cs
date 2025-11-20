using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoubleSword", menuName = "Weapon/DoubleSword")]
public class DoubleSword : Weapon
{
    protected override void OnAttack()
    {
        playerWeapon.animator.SetTrigger("attack");
    }

    protected override void OnSkill()
    {
        playerWeapon.animator.SetTrigger("skill");
    }

    protected override void SwitchingSkill()
    {
        playerWeapon.animator.SetTrigger("switching");
    }
}
