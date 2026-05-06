using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("BuffEffect/디버프 부여")]
public class AddDebuffEffect : BaseEffect
{
    [SerializeField]
    BuffData debuff;

    public override void OnEffect(BuffManager buffHandler)
    {
        Debug.Log(buffHandler);
        buffHandler.AddBuff(debuff);
    }
}
