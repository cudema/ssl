using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/공격 시")]
public class AttackEffect : UseEffect
{
    public override void OnApply(Player player)
    {
        useEffect.OnApply();
    }

    public override void OnRemove(Player player)
    {
        useEffect.OnRemove();
    }

    public override void OnEffect(BuffManager buffHandler)
    {
        useEffect.OnUseEffect(buffHandler);
    }
}
