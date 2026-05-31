using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoulStatAdder : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    TextMeshProUGUI goldText;
    [SerializeField]
    Transform toggles;
    [SerializeField]
    Toggle[] levelUI;
    int level = 0;

    [SerializeField]
    SoulUpgradeData data;
    [SerializeField]
    string statName;

    public void Setup()
    {
        levelUI = GetComponentsInChildren<Toggle>();
        level = PlayerPrefs.GetInt(statName);
        SetStat();
    }

    void OnEnable()
    {
        SetText();

        for (int i = 0; i < level; i++)
        {
            levelUI[i].isOn = true;
        }
    }

    public void LevelUp()
    {
        if (level >= levelUI.Length)
        {
            Debug.Log("IsMaxLevel");
            return;
        }

        if (EconomyManager.Instance.TrySpendSoul(data.soulDatas[level].useGold))
        {
            levelUI[level++].isOn = true;

            SetStat();
            
            PlayerPrefs.SetInt(statName, level);

            SetText();
        }
    }

    void SetText()
    {
        if (level >= levelUI.Length)
        {
            text.text = "Max";
            goldText.text = "Max";
            return;
        }
        goldText.text = "-" + data.soulDatas[level].useGold.ToString();

        if (data.isEffectRarityUp)
        {
            text.text = "증가량: 특성 확률 증가";
            return;
        }

        text.text = "증가량: " + data.soulDatas[level].upgradeValue.ToString();
    }

    void SetStat()
    {
        if (level == 0)
        {
            return;
        }

        if (data.isEffectRarityUp)
        {
            SoulManager.instance.SetRarityRange(data.rarityRanges[level - 1]);
        }
        else if (data.isGoldGetUp)
        {
            EconomyManager.Instance.UpgradeGoldAdd(data.soulDatas[level - 1].upgradeValue);
        }
        else
        {
            SoulManager.instance.SetSoulStat(data.type, data.soulDatas[level - 1].upgradeValue);
            Player.instance.OnPlayerStatReset();
        }

    }
}
