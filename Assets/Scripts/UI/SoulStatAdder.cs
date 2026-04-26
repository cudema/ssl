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
    Transform toggles;
    Toggle[] levelUI;
    int level = 0;

    [SerializeField]
    SoulUpgradeData data;

    void Start()
    {
        levelUI = GetComponentsInChildren<Toggle>();
    }

    public void LevelUp()
    {
        if (level >= levelUI.Length)
        {
            Debug.Log("IsMaxLevel");
            return;
        }
        if (data.isEffectRarityUp)
        {
            SoulManager.instance.SetRarityRange(data.rarityRanges[level]);
            return;
        }
        if (data.isGoldGetUp)
        {
            return;
        }

        if (EconomyManager.Instance.TrySpendSoul(data.soulDatas[level].useGold))
        {
            if (data.isGoldGetUp)
            {
                EconomyManager.Instance.UpgradeGoldAdd(data.soulDatas[level].upgradeValue);
                return;
            }
            
            SoulManager.instance.SetSoulStat(data.type, data.soulDatas[level].upgradeValue);
            levelUI[level++].isOn = true;
        }
    }
}
