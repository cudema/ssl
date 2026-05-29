using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelrectUI : UIBase
{
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
}
