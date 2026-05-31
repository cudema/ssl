using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulUI : UIBase
{
    [SerializeField]
    SoulStatAdder[] soulStatAdders;

    public override void OnUI()
    {
        base.OnUI();
        UIManager.instance.gameMenuUI.isOnable = false;
    }

    public override void OffUI()
    {
        base.OffUI();
        UIManager.instance.gameMenuUI.isOnable = true;
    }

    void Start()
    {
        foreach (SoulStatAdder temp in soulStatAdders)
        {
            temp.Setup();
        }
    }
}
