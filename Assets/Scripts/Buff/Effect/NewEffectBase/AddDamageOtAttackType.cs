using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/공격 별 피해량 증가")]
public class AddDamageOtAttackType : IAttackAddDamageEffect
{
    [SerializeField]
    AttackType attackType;
    [SerializeField]
    int attackIndex;
    [SerializeField]
    float value;

    public void OnApply(Player player)
    {
        
    }

    public void OnRemove(Player player)
    {
        
    }

    public float OnAttackAddDamageEffect()
    {
        WeaponAttackData data = Player.instance.GetCurrentAttackData();
        
        if (data.DamageType == attackType)
        {
            if (attackIndex < 0)
            {
                return value;
            }
            else if (attackIndex == data.index)
            {
                return value;
            }
        }
        return 0;
    }
}
