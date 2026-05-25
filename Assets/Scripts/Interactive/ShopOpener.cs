using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpener : InteractiveObject
{
    [SerializeField]
    RarityRange rarityRange;

    public void SetRarityRange(RarityRange rarityRange)
    {
        this.rarityRange = rarityRange;
    }

    void OnDisable()
    {
        
    }

    protected override void OnAction()
    {
        UIManager.instance.shop.OnShop(rarityRange);
    }
}
