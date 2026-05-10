using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/완벽 회피 성공 시")]
public class SuccessPerfactAvoidance : UseEffect
{
    public override void OnApply(Player player)
    {
        useEffect.OnApply();
        
    }

    public override void OnRemove(Player player)
    {
        useEffect.OnRemove();
    }

    public override void OnEffect(BuffManager buffManager)
    {
        useEffect.OnUseEffect(buffManager);
    }
}
