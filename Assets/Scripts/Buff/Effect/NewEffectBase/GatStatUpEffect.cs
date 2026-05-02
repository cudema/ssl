using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/스탯 강화")]
public class GatStatUpEffect : IEffect
{
    [SerializeField]
    BuffModifier addStat;

    public void OnApply(Player player)
    {
        player.playerStats.AddStat(addStat);
    }

    public void OnRemove(Player player)
    {
        player.playerStats.RemoveStat(addStat);
    }
}
