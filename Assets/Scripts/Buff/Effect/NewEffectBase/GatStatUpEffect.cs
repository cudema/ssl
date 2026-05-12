using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/스탯 강화")]
public class GatStatUpEffect : Effect
{
    [SerializeField]
    BuffModifier addStat;

    public override void OnApply(Player player)
    {
        Player.instance.playerStats.AddStat(addStat);
        foreach (AddValue temp in addStat.addValues)
        {
            Player.instance.SetStat(temp.targetStat);
        }
    }

    public override void OnRemove(Player player)
    {
        Player.instance.playerStats.RemoveStat(addStat);
        foreach (AddValue temp in addStat.addValues)
        {
            Player.instance.SetStat(temp.targetStat);
        }
    }
}
