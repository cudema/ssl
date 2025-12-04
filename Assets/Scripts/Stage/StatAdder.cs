using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    HP = 0,
    Defence,
    AttackDamage,
    CriticalRange,
    CriticalDamage,
    Penetration
}

[Serializable]
public struct AdderStatData
{
    [SerializeField]
    Stat stat;
    [SerializeField]
    public Sprite icon;
    [SerializeField]
    public float value;
}

public class StatAdder : MonoBehaviour
{
    [SerializeField]
    AdderStatData[] adderStats;

    public void AddStat(Stat addStat)
    {
        switch(addStat)
        {
            case Stat.HP:
                Player.instance.HpBonus += adderStats[(int)Stat.HP].value;
                break;
            case Stat.Defence:
                Player.instance.DefenseBonus += adderStats[(int)Stat.Defence].value;
                break;
            case Stat.AttackDamage:
                Player.instance.AttackDamageBonus += adderStats[(int)Stat.AttackDamage].value;
                break;
            case Stat.CriticalRange:
                Player.instance.CriticalRangeBonus += adderStats[(int)Stat.CriticalRange].value;
                break;
            case Stat.CriticalDamage:
                Player.instance.CriticalDamageBonus += adderStats[(int)Stat.CriticalDamage].value;
                break;
            case Stat.Penetration:
                Player.instance.PenetrationBonus += adderStats[(int)Stat.Penetration].value;
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }

    void SetStat()
    {
        List<AdderStatData> tempList = new List<AdderStatData>(adderStats);

        int index = UnityEngine.Random.Range(0, tempList.Count);
        AdderStatData data0 = tempList[index];
        tempList.RemoveAt(index);
        
        index = UnityEngine.Random.Range(0, tempList.Count);
        AdderStatData data1 = tempList[index];
        tempList.RemoveAt(index);

        index = UnityEngine.Random.Range(0, tempList.Count);
        AdderStatData data2 = tempList[index];
        tempList.RemoveAt(index);

        
    }
}
