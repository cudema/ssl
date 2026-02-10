using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecompositionEffect : IAttackEffect
{
    public void OnApply(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<PoisonEffect>().ChangePoisonBuff(Resources.Load<Poison>("Buff/UpgradePoison"));
    }

    public void OnRemove(Player player)
    {

    }

    public void OnAttackEffect(BuffHandler enemy)
    {
        
    }
}
