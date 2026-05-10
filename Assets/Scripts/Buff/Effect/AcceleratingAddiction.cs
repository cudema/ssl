using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AcceleratingAddiction : AddValueEffect
{
    public override void OnApply(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<PoisonEffect>().ChangePoisonValue(-0.3f);
    }

    public override void OnRemove(Player player)
    {
        player.GetComponent<PlayerEffectHandler>().FindEffect<PoisonEffect>().ChangePoisonValue(0.3f);
    }

    public override float OnEffect(BuffManager enemy)
    {
        if (!enemy.ChackActiveBuff(50)) return 0;

        return enemy.GetBuffSttack(50) * 0.015f;
    }
}
