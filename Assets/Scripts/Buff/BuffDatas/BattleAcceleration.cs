using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Buff System/BattleAcceleration")]
public class BattleAcceleration : BuffData
{
    public override void OnBuffEffect(BuffManager buffManager)
    {
        base.OnBuffEffect(buffManager);
        Player.instance.isBattleAcceleration = true;
        //Player.instance.SwitchingGauge = 0;
    }

    public override void OffBuffEffect(BuffManager buffManager)
    {
        base.OffBuffEffect(buffManager);
        Player.instance.isBattleAcceleration = false;
    }
}
