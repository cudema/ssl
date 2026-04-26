using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoulData", menuName = "SoulData/Data")]
public class SoulUpgradeData : ScriptableObject
{
    public bool isGoldGetUp;
    public bool isEffectRarityUp;
    public StatType type;
    [SerializeField]
    public SoulData[] soulDatas;
    public RarityRange[] rarityRanges;
}

[System.Serializable]
public struct SoulData
{
    [SerializeField]
    public int useGold;
    [SerializeField]
    public float upgradeValue;
}
