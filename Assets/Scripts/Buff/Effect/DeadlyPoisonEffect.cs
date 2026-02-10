using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyPoisonEffect : IEffect
{
    public void OnApply(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<PoisonEffect>().Poison0.duration *= 2;
    }

    public void OnRemove(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<PoisonEffect>().Poison0.duration /= 2;
    }
}
