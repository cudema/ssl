using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulManager : MonoBehaviour
{
    public static SoulManager instance;
    public Dictionary<StatType, float> soulStats = new Dictionary<StatType, float>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        soulStats[StatType.HP] = 0;
        soulStats[StatType.Defence] = 0;
        soulStats[StatType.AttackDamage] = 0;
        soulStats[StatType.CriticalDamage] = 0;
        soulStats[StatType.CriticalRange] = 0;
        soulStats[StatType.Penetration] = 0;
    }

    public void SetSoulStat(StatType type, float value)
    {
        soulStats[type] = value;
    }
}
