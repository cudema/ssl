using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHPEffect : IAttackEffect, IHPChanged
{
    public event Action<float> ChangedGrayHp;

    float grayHp;

    public float GrayHp
    {
        set
        {
            float temp = grayHp;
            grayHp = Mathf.Clamp(value, 0, Player.instance.MaxHp);
            ChangedGrayHp?.Invoke(grayHp - temp);
        }
        get => grayHp;
    }

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
        GrayHp -= value;
    }
}
