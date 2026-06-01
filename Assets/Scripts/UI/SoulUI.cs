using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulUI : UIBase
{
    [SerializeField]
    SoulStatAdder[] soulStatAdders;

    public override void OnUI()
    {
        Start();
        base.OnUI();
    }

    public override void OffUI()
    {
        base.OffUI();
    }

    void Start()
    {
        foreach (SoulStatAdder temp in soulStatAdders)
        {
            temp.Setup();
        }
    }
}
