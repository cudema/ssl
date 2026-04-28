using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }
}
