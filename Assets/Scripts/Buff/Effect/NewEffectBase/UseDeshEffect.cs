using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("NewEffect/회피 시")]
public class UseDeshEffect : IDeshEffect
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
    public void OnDeshEffect(BuffManager enemy)
    {
        useEffect.OnEffect(enemy);
    }
}
