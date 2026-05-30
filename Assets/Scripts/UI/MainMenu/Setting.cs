using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : UIBase
{
    [SerializeField]
    Slider master;
    [SerializeField]
    Slider BGM;
    [SerializeField]
    Slider SFX;
    [SerializeField]
    Slider mouseReception;

    public void SetMaster()
    {
        SoundManager.instance.SetMasterVolume(master.value);
    }

    public void SetBGM()
    {
        SoundManager.instance.SetBGMVolume(BGM.value);
    }

    public void SetSFX()
    {
        SoundManager.instance.SetSFXVolume(SFX.value);
    }

    public void SetMaous()
    {
        Player.instance.movement.addAngleSpeed = mouseReception.value;
        PlayerPrefs.SetFloat("MaousRec", Player.instance.movement.addAngleSpeed);
    }

    public void SettingOnUI()
    {
        if (UI.activeSelf)
        {
            OffUI();
            return;
        }
        
        OnUI();
    }

    public override void OnUI()
    {
        base.OnUI();

        float temp;
        SoundManager.instance.audioMixer.GetFloat("Master", out temp);
        Debug.Log(temp);
        if (master.value != temp)
        {
            master.value = temp;
        }
        SoundManager.instance.audioMixer.GetFloat("BGM", out temp);
        Debug.Log(temp);
        if (BGM.value != temp)
        {
            BGM.value = temp;
        }
        SoundManager.instance.audioMixer.GetFloat("SFX", out temp);
        Debug.Log(temp);
        if (SFX.value != temp)
        {
            SFX.value = temp;
        }

        temp = PlayerPrefs.GetFloat("MaousRec");
        if (mouseReception.value != temp)
        {
            mouseReception.value = temp;
        }
    }

    public override void OffUI()
    {
        UI.SetActive(false);
    }
}
