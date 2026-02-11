using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHPEffect : IAttackEffect, IHPChanged
{
    public void OnApply(Player player)
    {
        player.ChangedHp += ChangedHP;
    }

    public void OnRemove(Player player)
    {
        player.ChangedHp -= ChangedHP;
    }

    public void OnAttackEffect(BuffHandler enemy)
    {
        
    }

    public void ChangedHP(float value)
    {
        Player.instance.GrayHp -= value;
    }
}
