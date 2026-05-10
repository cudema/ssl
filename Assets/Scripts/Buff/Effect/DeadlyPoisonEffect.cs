using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("OldEffect/독 시간 감소")]
public class DeadlyPoisonEffect : Effect
{
    public override void OnApply(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<PoisonEffect>().Poison0.duration *= 2;
    }

    public override void OnRemove(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<PoisonEffect>().Poison0.duration /= 2;
    }
}
