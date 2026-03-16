using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpener : InteractiveObject
{
    [SerializeField]
    RarityRange rarityRange;

    protected override void OnAction()
    {
        UIManager.instance.shop.OnShop(rarityRange);
    }
}
