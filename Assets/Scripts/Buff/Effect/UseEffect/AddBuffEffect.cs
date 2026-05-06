using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("BuffEffect/버프 획득")]
public class AddBuffEffect : BaseEffect
{
    [SerializeField]
    BuffData buff;

    public override void OnEffect(BuffManager buffHandler)
    {
        Player.instance.buffManager.AddBuff(buff);
    }
}

