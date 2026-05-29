using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartObj : InteractiveObject
{
    protected override void OnAction()
    {
        UIManager.instance.weaponSelrect.GetComponent<StartGameControll>().StartGame();        
        isInteractiable = false;
        OnInteractionEvent?.Invoke();
    }
}
