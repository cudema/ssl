using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject UI;

    public virtual void OnUI()
    {
        UI.SetActive(true);
        Player.instance.StopPlayer();
    }

    public virtual void OffUI()
    {
        UI.SetActive(false);
        Player.instance.SetupPlayer();
        InputManager.instance.StartControll();
    }
}
