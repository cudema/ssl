using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartButten : MonoBehaviour
{
    [SerializeField]
    Weapon weapon;

    public void GameStart()
    {
        SceneControlManager.instance.LoadScene(SceneName.StartStage);

        Player.instance.OnPositionSet(new Vector3(0, 1f, 0));
        Player.instance.playerInputController.Setup();
        Player.instance.OnPlayerStatReset();
        Player.instance.SetupPlayer();
        Player.instance.SetupWeapon(weapon, weapon);
    }
}
