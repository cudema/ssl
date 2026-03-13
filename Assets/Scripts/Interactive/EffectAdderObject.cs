using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAdderObject : InteractiveObject
{
    [SerializeField]
    RarityRange rarityRange;

    protected override void OnAction()
    {
        UIManager.instance.effectAdder.SetEffect(rarityRange);
    }
}
