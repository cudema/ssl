using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/회피 시")]
public class UseDeshEffect : UseEffect
{
    public override void OnApply(Player player)
    {
        useEffect.OnApply();
    }
    public override void OnRemove(Player player)
    {
        useEffect.OnRemove();
    }
    public override void OnEffect(BuffManager enemy)
    {
        useEffect.OnUseEffect(enemy);
    }
}
