using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AcceleratingAddiction : IAttackAddDamageEffect
{
    public void OnApply(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<PoisonEffect>().ChangePoisonValue(-0.3f);
    }

    public void OnRemove(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<PoisonEffect>().ChangePoisonValue(0.3f);
    }

    public float OnEffect(BuffManager enemy)
    {
        if (!enemy.ChackActiveBuff(50)) return 0;

        return enemy.GetBuffSttack(50) * 0.015f;
    }
}
