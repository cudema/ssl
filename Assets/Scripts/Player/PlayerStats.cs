using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public List<StatEntry> initialStats = new List<StatEntry>();

    public Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();

    [System.Serializable]
    public class StatEntry
    {
        public StatType type;
        public float baseValue;
    }

    void Awake()
    {
        foreach (var entry in initialStats)
        {
            stats[entry.type] = new Stat { baseValue = entry.baseValue };
        }
    }
}
