using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rapier", menuName = "Weapon/Rapier")]
public class Rapier : Weapon
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
