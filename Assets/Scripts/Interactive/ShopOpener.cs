using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpener : InteractiveAction
{
    [SerializeField]
    RarityRange rarityRange;
    public bool IsInteractiable
    {
        get => interObj.IsInteractiable;
    }
    InteractiveObject interObj;

    void Awake()
    {
        interObj = GetComponent<InteractiveObject>();
    }

    public void SetRarityRange(RarityRange rarityRange)
    {
        this.rarityRange = rarityRange;
    }

    void OnDisable()
    {
        UIManager.instance.shop.ResetShop();
    }

    void OnDestroy()
    {
        UIManager.instance.shop.ResetShop();
    }

    public override void OnAction()
    {
        UIManager.instance.shop.OnShop(rarityRange);
    }
}
