using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAdderObject : InteractiveObject
{
    [SerializeField]
    bool isNotSetRarity = false;
    [SerializeField]
    RarityRange rarityRange;

    public void SetRarityRange(RarityRange rarityRange)
    {
        if (isNotSetRarity) return;
        this.rarityRange = rarityRange;
    }

    protected override void OnAction()
    {
        OnInteractionEvent?.Invoke();
        UIManager.instance.effectAdder.SetEffect(rarityRange);
        UIManager.instance.effectAdder.AddedEffect += AddEffect;
    }

    void AddEffect()
    {
        isInteractiable = false;
        UIManager.instance.effectAdder.AddedEffect -= AddEffect;
    }
}
