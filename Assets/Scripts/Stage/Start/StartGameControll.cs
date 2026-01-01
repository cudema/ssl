using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameControll : MonoBehaviour
{
    [SerializeField]
    Weapon[] weapons;

    Weapon mainWeapon;
    Weapon subWeapon;

    [SerializeField]
    ToggleGroup mainGroup;

    [SerializeField]
    ToggleGroup subGroup;

    Toggle[] mainToggles;
    Toggle[] subToggles;

    void Awake()
    {
        subToggles = subGroup.GetComponentsInChildren<Toggle>();
        mainToggles = mainGroup.GetComponentsInChildren<Toggle>();
    }

    public void SetMainWeapon(int index)
    {
        mainWeapon = weapons[index];

        subToggles[0].interactable = true;
        subToggles[1].interactable = true;
        subToggles[2].interactable = true;

        if (mainToggles[index].isOn)
        {
            subToggles[index].interactable = false;
            subToggles[index].isOn = false;
        }
    }

    public void SetSubWeapon(int index)
    {
        subWeapon = weapons[index];
    }

    public void StartGame()
    {
        if (mainWeapon == null || subWeapon == null)
        {
            Debug.Log("실행 실패");
            return;
        }

        Player.instance.StartCoroutine(StartingGame());
    }

    IEnumerator StartingGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene");
        asyncLoad.allowSceneActivation = false;


        while (!asyncLoad.isDone)
        {
            Debug.Log(asyncLoad.progress + "%");

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        yield return null;

        Player.instance.OnPositionSet(new UnityEngine.Vector3(0, 1f, 0));
        Player.instance.SetupWeapon(mainWeapon, subWeapon);
    }
}
