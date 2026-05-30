using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/스위칭 게이지 사용량 감소")]
public class UseSwitchingGageChange : Effect
{
    [SerializeField]
    int value;
    
    public override void OnApply(Player player)
    {
        useEffect?.OnApply();
        Player.instance.playerWeapon.useSwitchingGauge -= value;
    }

    public override void OnRemove(Player player)
    {
        useEffect?.OnRemove();
        Player.instance.playerWeapon.useSwitchingGauge += value;
    }
}
