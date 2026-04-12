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
    SelecterToggle mainToggle;

    [SerializeField]
    ToggleGroup subGroup;
    [SerializeField]
    SelecterToggle subToggle;

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
            mainToggle.SetImage(index);
            return;
        }
        mainWeapon = null;
        mainToggle.SetImage(-1);
    }

    public void SetSubWeapon(int index)
    {
        if (!subToggles[index].isOn)
        {
            subWeapon = null;
            subToggle.SetImage(-1);
            return;
        }

        subWeapon = weapons[index];
        subToggle.SetImage(index);
    }

    public void StartGame()
    {
        if (mainWeapon == null || subWeapon == null)
        {
            Debug.Log("실행 실패");
            return;
        }
        //UIManager.instance.weaponSelrect.OffUI();
        Player.instance.StartCoroutine(StartingGame());
    }

    public void SetWeapon()
    {
        if (mainWeapon == null || subWeapon == null)
        {
            Debug.Log("실행 실패");
            return;
        }
        Player.instance.SetupWeapon(mainWeapon, subWeapon);
    }

    IEnumerator StartingGame()
    {
        yield return StartCoroutine(SceneControlManager.instance.Loading(SceneName.Stage_1));

        StageManager.instance.StartScene();
        Player.instance.SetupWeapon(mainWeapon, subWeapon);
    }
}
