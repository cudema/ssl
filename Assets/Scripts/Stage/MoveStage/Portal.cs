using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : InteractiveObject
{
    [SerializeField]
    SceneName[] sceneName;

    protected override void OnAction()
    {
        isInteractiable = false;
        OnInteractionEvent?.Invoke();
        SceneControlManager.instance.LoadScene(sceneName[Random.Range(0, sceneName.Length)]);
    }
}
