using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UpgradePoison", menuName = "Buff System/UpgradePoison")]
public class UpgradePoison : Poison
{
    public override void OnBuffEffect(BuffManager buffManager)
    {
        if (buffManager.ChackActiveBuff(181818))
        {
            buffManager.EndBuff(this);
        }
    }

    public override void OffBuffEffect(BuffManager buffManager)
    {
        buffManager.AddBuff(Resources.Load<BuffData>("Buff/Dfender"));
    }
}
