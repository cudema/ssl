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
        UIManager.instance.effectAdder.AddedEffect += AddEffect;
    }

    void AddEffect()
    {
        isInteractiable = false;
        UIManager.instance.effectAdder.AddedEffect -= AddEffect;
    }
}
