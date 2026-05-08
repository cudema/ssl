using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/공격 시")]
public class GetBuffOfAttackEffect : IAttackEffect
{
    [SerializeReference, SubclassSelector]
    BaseEffect useEffect;

    public void OnApply(Player player)
    {
        useEffect.OnApply();
    }

    public void OnRemove(Player player)
    {
        useEffect.OnRemove();
    }

    public void OnEffect(BuffManager buffHandler)
    {
        useEffect.OnUseEffect(buffHandler);
    }
}
