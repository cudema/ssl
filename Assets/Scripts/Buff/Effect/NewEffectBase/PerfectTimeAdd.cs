using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/완벽회피 시간 연장")]
public class PerfectTimeAdd : Effect
{
    public override void OnApply(Player player)
    {
        useEffect?.OnApply();
        Player.instance.playerWeapon.perfectAvoidTime = 0.2f;
    }

    public override void OnRemove(Player player)
    {
        useEffect?.OnRemove();
        Player.instance.playerWeapon.perfectAvoidTime = 0.01f;
    }
}
