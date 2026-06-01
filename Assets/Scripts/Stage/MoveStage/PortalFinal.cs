using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalFinal : Portal
{
    protected override void OnAction()
    {
        isInteractiable = false;
        OnInteractionEvent?.Invoke();
        StageManager.instance.EndRun(sceneName[Random.Range(0, sceneName.Length)]);
    }
}
