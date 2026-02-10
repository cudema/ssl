using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratingAddiction : IAttackAddDamagePerEffect
{
    public void OnApply(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<PoisonEffect>().ChangePoisonValue(-0.3f);
    }

    public void OnRemove(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<PoisonEffect>().ChangePoisonValue(0.3f);
    }

    public float OnAttackAddDamagePerEffect(BuffHandler enemy)
    {
        if (!enemy.ChackActiveBuff(50)) return 0;

        return enemy.GetBuffSttack(50) * 0.015f;
    }
}
