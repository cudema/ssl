using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuUI : UIBase
{
    [SerializeField]
    Toggle menuToggle;
    [SerializeField]
    Toggle inventoryToggle;
    [SerializeField]
    Toggle mapToggle;

    int currentIndex = 0;

    public override void OnUI()
    {
        base.OnUI();
        
        if (!isOnable) return;
        
        switch (currentIndex)
        {
            case 0:
                menuToggle.isOn = true;
                break;
            case 1:
                inventoryToggle.isOn = true;
                break;
            case 2:
                mapToggle.isOn = true;
                break;
            default:
                menuToggle.isOn = true;
                break;
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == SceneName.StartMenu.ToString()) return;

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (UI.activeSelf)
            {
                OffUI();
                if (menuToggle.isOn)
                {
                    menuToggle.isOn = false;
                    currentIndex = 0;
                    return;
                }
                if (inventoryToggle.isOn)
                {
                    inventoryToggle.isOn = false;
                    currentIndex = 1;
                    return;
                }
                if (mapToggle.isOn)
                {
                    mapToggle.isOn = false;
                    currentIndex = 2;
                    return;
                }
            }
            else
            {
                OnUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (menuToggle.isOn)
            {
                OffUI();

                menuToggle.isOn = false;
                currentIndex = 0;
                return;
            }
            else
            {
                menuToggle.isOn = false;
                inventoryToggle.isOn = false;
                mapToggle.isOn = false;
                currentIndex = 0;

                OnUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryToggle.isOn)
            {
                OffUI();

                inventoryToggle.isOn = false;
                currentIndex = 1;
                return;
            }
            else
            {
                menuToggle.isOn = false;
                inventoryToggle.isOn = false;
                mapToggle.isOn = false;
                currentIndex = 1;

                OnUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mapToggle.isOn)
            {
                OffUI();

                mapToggle.isOn = false;
                currentIndex = 2;
                return;
            }
            else
            {
                menuToggle.isOn = false;
                inventoryToggle.isOn = false;
                mapToggle.isOn = false;
                currentIndex = 2;

                OnUI();
            }
        }
    }
}
