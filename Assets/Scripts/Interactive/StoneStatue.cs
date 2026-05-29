using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneStatue : InteractiveObject
{
    protected override void OnAction()
    {        
        isInteractiable = false;
        OnInteractionEvent?.Invoke();
        StartCoroutine(WeaponUpgrade());
    }

    IEnumerator WeaponUpgrade()
    {
        Player.instance.StopPlayer();
        
        UIManager.instance.statAdder.SetStat();

        yield return new WaitWhile(() => UIManager.instance.statAdder.isSelectingStat);

        Player.instance.SetupPlayer();
    }
}
