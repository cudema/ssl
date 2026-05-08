using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("OldEffect/버프가 있으면 체력회복")]
public class SurvivalTechniqueEffect : IAttackEffect
{
    public void OnApply(Player player)
    {
        
    }

    public void OnRemove(Player player)
    {
        
    }

    public void OnEffect(BuffManager enemy)
    {
        if (enemy.ChackActiveBuff(50))
        {
            Player.instance.CurrentHp += Player.instance.MaxHp * 0.005f;
        }
    }
}
