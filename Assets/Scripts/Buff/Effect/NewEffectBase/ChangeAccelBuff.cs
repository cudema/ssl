using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/스위칭 버프 수정")]
public class ChangeAccelBuff : Effect
{
    [SerializeField]
    float addDuration;
    [SerializeField]
    AddValue addValue;
    [SerializeField]
    int addStack;
    [SerializeField]
    int addSwitchingGauge = 0;

    public override void OnApply(Player player)
    {
        Player.instance.useAccelBuff.duration += addDuration;
        Player.instance.useAccelBuff.maxStack += addStack;
        Player.instance.useAccelBuff.addGauge += addSwitchingGauge;
        if (addValue.value != 0)
        {
            AddValue[] temp = new AddValue[Player.instance.useAccelBuff.addValues.Length + 1];
            temp[0] = addValue;
            for (int i = 1; i < temp.Length; i++)
            {
                temp[i] = Player.instance.useAccelBuff.addValues[i - 1];
            }
        }
    }
    public override void OnRemove(Player player)
    {
        Player.instance.useAccelBuff.duration -= addDuration;
        Player.instance.useAccelBuff.maxStack -= addStack;
        Player.instance.useAccelBuff.addGauge -= addSwitchingGauge;
        if (addValue.value != 0)
        {
            AddValue[] temp = new AddValue[Player.instance.useAccelBuff.addValues.Length - 1];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = Player.instance.useAccelBuff.addValues[i + 1];
            }
        }
    }
}
