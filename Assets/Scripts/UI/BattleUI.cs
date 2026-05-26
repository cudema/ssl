using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : UIBase
{
    public override void OnUI()
    {
        UI.SetActive(true);
        Player.instance.SetupPlayer();
        InputManager.instance.StartControll();
    }

    public override void OffUI()
    {
        UI.SetActive(false);
        Player.instance.StopPlayer();
    }
}
