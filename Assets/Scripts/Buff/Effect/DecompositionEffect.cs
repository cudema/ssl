using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("OldEffect/독 강화")]
public class DecompositionEffect : UseEffect
{
    public override void OnApply(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<PoisonEffect>().ChangePoisonBuff(Resources.Load<Poison>("Buff/UpgradePoison"));
    }

    public override void OnRemove(Player player)
    {

    }

    public override void OnEffect(BuffManager enemy)
    {
        
    }
}
