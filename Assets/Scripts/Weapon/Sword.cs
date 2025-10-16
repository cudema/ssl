using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Weapon/Sword")]
public class Sword : Weapon
{
    protected override void OnAttack()
    {
        playerWeapon.animator.SetTrigger("attack");
    }

    protected override void OnSkill()
    {

    }
}
