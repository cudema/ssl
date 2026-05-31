using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : UIBase
{
    void Update()
    {
        if (UIManager.instance.gameMenuUI.UI.activeSelf) return;
    }

    public void OnSetting()
    {
        UIManager.instance.setting.OnUI();
    }

    public void GoToMain()
    {
        OffUI();
        StageManager.instance.EndRun(SceneName.StartMenu);
    }

    public override void OnUI()
    {
        if (!isOnable) return;
        UI.SetActive(true);
        UIManager.instance.gameMenuUI.isOnable = false;
        Player.instance.StopPlayer();
    }

    public override void OffUI()
    {
        UI.SetActive(false);
        Player.instance.SetupPlayer();
        UIManager.instance.gameMenuUI.isOnable = true;
        UIManager.instance.setting.OffUI();
        InputManager.instance.StartControll();
    }
}
