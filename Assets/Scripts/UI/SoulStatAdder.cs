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
    Toggle[] levelUI;
    int level = 0;

    [SerializeField]
    SoulUpgradeData data;

    void Start()
    {
        levelUI = GetComponentsInChildren<Toggle>();
        text.text = "증가량: " + data.soulDatas[level].upgradeValue.ToString();
        goldText.text = "-" + data.soulDatas[level].useGold.ToString();
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
            if (data.isEffectRarityUp)
            {
                SoulManager.instance.SetRarityRange(data.rarityRanges[level++]);
            }
            else if (data.isGoldGetUp)
            {
                EconomyManager.Instance.UpgradeGoldAdd(data.soulDatas[level++].upgradeValue);
            }
            else
            {
                SoulManager.instance.SetSoulStat(data.type, data.soulDatas[level].upgradeValue);
            }

            levelUI[level++].isOn = true;
            text.text = data.soulDatas[level].upgradeValue.ToString();
            goldText.text = data.soulDatas[level].useGold.ToString();
            
            if (level >= levelUI.Length)
            {
                text.text = "Max";
                goldText.text = "Max";
            }
        }
    }
}
