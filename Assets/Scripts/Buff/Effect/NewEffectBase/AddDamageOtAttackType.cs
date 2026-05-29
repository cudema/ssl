using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/공격 별 피해량 증가")]
public class AddDamageOtAttackType : AddValueEffect
{
    [SerializeField]
    AttackType attackType;
    [SerializeField]
    bool isDesignateIndex;
    [SerializeField]
    int attackIndex;
    [SerializeField]
    float value;

    public override void OnApply(Player player)
    {
        useEffect?.OnApply();
    }

    public override void OnRemove(Player player)
    {
        useEffect?.OnRemove();
    }

    public override float OnEffect(BuffManager buffManager)
    {
        WeaponAttackData data = Player.instance.GetCurrentAttackData();
        
        if (data.DamageType == attackType)
        {
            if (!isDesignateIndex)
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

[System.Serializable]
[AddTypeMenu("NewEffect/특정 공격에 효과 발동")]
public class AttackTypeToEffect : UseEffect
{
    [SerializeField]
    AttackType attackType;
    [SerializeField]
    int attackIndex;

    public override void OnApply(Player player)
    {
        useEffect.OnApply();
    }

    public override void OnRemove(Player player)
    {
        useEffect.OnRemove();
    }

    public override void OnEffect(BuffManager buffManager)
    {
        WeaponAttackData data = Player.instance.GetCurrentAttackData();
        
        if (data.DamageType == attackType)
        {
            if (attackIndex == data.index)
            {
                useEffect.OnEffect(buffManager);
            }
        }
    }
}