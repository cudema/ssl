using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject UI;

    public bool isOnable = true;

    public virtual void OnUI()
    {
        if (!isOnable) return;
        UI.SetActive(true);
        UIManager.instance.gameMenuUI.isOnable = false;
        Player.instance.StopPlayer();
    }

    public virtual void OffUI()
    {
        UI.SetActive(false);
        Player.instance.SetupPlayer();
        UIManager.instance.gameMenuUI.isOnable = true;
        InputManager.instance.StartControll();
    }
}
