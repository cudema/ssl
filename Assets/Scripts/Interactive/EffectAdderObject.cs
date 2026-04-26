using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAdderObject : InteractiveObject
{
    [SerializeField]
    RarityRange rarityRange;

    public void SetRarityRange(RarityRange rarityRange)
    {
        this.rarityRange = rarityRange;
    }

    protected override void OnAction()
    {
        UIManager.instance.effectAdder.SetEffect(rarityRange);
    }
}
