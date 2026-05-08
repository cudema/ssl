using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/회피 성공 시")]
public class SuccessEvasionEffect : ISuccessEvasionEffect
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

    public void OnEffect(BuffManager buffManager)
    {
        useEffect.OnUseEffect(buffManager);
    }
}
