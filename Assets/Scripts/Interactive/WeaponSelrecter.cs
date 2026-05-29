using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelrecter : InteractiveObject
{
    protected override void OnAction()
    {
        isInteractiable = false;
        OnInteractionEvent?.Invoke();
        UIManager.instance.weaponSelrect.OnUI();
    }
}
