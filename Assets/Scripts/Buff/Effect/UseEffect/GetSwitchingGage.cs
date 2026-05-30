using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("스위칭 게이지 획득")]
public class GetSwitchingGage : BaseEffect
{
    [SerializeField]
    int value;

    public override void OnApply()
    {
        
    }

    public override void OnRemove()
    {
        
    }

    public override void OnEffect(BuffManager enemy)
    {
        Player.instance.SwitchingGauge += value;
    }
}
