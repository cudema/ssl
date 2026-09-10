using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelrecter : InteractiveAction
{
    public override void OnAction()
    {
        UIManager.instance.weaponSelrect.OnUI();
    }
}
