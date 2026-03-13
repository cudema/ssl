using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SurvivalTechniqueEffect : IAttackEffect
{
    public void OnApply(Player player)
    {
        
    }

    public void OnRemove(Player player)
    {
        
    }

    public void OnAttackEffect(BuffHandler enemy)
    {
        if (enemy.ChackActiveBuff(50))
        {
            Player.instance.CurrentHp += Player.instance.MaxHp * 0.005f;
        }
    }
}
