using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartButten : MonoBehaviour
{
    [SerializeField]
    Weapon weapon;

    [SerializeField]
    Weapon sword;
    [SerializeField]
    Weapon axe;

    public void GameStart()
    {
        if (PlayerPrefs.GetInt("PlayGame") == 0)
        {
            PlayerPrefs.SetInt("PlayGame", 1);
            SceneControlManager.instance.LoadScene(SceneName.Stage_Tutorial);
            Player.instance.OnPlayerStatReset();
            Player.instance.SetupPlayer();
            Player.instance.SetupWeapon(sword, axe);

            return;
        }

        SceneControlManager.instance.LoadScene(SceneName.StartStage);

        Player.instance.playerInputController.Setup();
        Player.instance.OnPlayerStatReset();
        Player.instance.SetupPlayer();
        Player.instance.SetupWeapon(weapon, weapon);
    }

    public void OnSetting()
    {
        UIManager.instance.setting.OnUI();
    }
}
