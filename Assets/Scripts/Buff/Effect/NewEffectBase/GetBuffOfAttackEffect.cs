using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/공격 시 버프 획득")]
public class GetBuffOfAttackEffect : IAttackEffect
{
    [SerializeField]
    BuffData buff;

    public void OnApply(Player player)
    {
        
    }

    public void OnRemove(Player player)
    {
        
    }

    public void OnAttackEffect(BuffManager buffHandler)
    {
        Player.instance.buffManager.AddBuff(buff);
    }
}
