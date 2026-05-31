using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StatEntry
{
    public StatType type;
    public float baseValue;

    public StatEntry(StatType type, float value)
    {
        this.type = type;
        baseValue = value;
    }
}

[System.Serializable]
public class PlayerStats : MonoBehaviour
{
    public List<StatEntry> initialStats = new List<StatEntry>();

    public Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();

    void Awake()
    {
        foreach (var entry in initialStats)
        {
            stats[entry.type] = new Stat { baseValue = entry.baseValue };
        }
    }

    public void OnStatsReset()
    {
        foreach (var entry in initialStats)
        {
            stats[entry.type].baseValue = entry.baseValue + SoulManager.instance.soulStats[entry.type];
            stats[entry.type].ForceDirty();

            Debug.Log(SoulManager.instance.soulStats[entry.type]);
        }
    }

    public void OnReset()
    {
        foreach (var entry in initialStats)
        {
            stats[entry.type].baseValue = entry.baseValue;
            stats[entry.type].ForceDirty();
        }
    }

    public void AddStat(BuffModifier buffModifier)
    {
        foreach(AddValue addValue in buffModifier.addValues)
        {
            stats[addValue.targetStat].AddModifier(buffModifier);
        }
    }

    public void RemoveStat(BuffModifier buffModifier)
    {
        foreach(AddValue addValue in buffModifier.addValues)
        {
            stats[addValue.targetStat].RemoveModifier(buffModifier);
        }
    }
}