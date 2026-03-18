using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulManager : MonoBehaviour
{
    public static SoulManager instance;
    List<StatEntry> soulStats = new List<StatEntry>();

    

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
        soulStats.Add(new StatEntry(StatType.HP, 0));
        soulStats.Add(new StatEntry(StatType.AttackDamage, 0));
        soulStats.Add(new StatEntry(StatType.Defence, 0));
        soulStats.Add(new StatEntry(StatType.CriticalDamage, 0));
        soulStats.Add(new StatEntry(StatType.CriticalRange, 0));
        soulStats.Add(new StatEntry(StatType.Penetration, 0));
    }

    public void SetSoulStat(StatType type, float value)
    {
        foreach (var temp in soulStats)
        {
            if (temp.type == type)
            {
                temp.baseValue = value;
                return;
            }
        }
    }
}
