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
    public Stat stat;
    [SerializeField]
    public Sprite icon;
    [SerializeField]
    public float value;
    [SerializeField]
    public string tooltip;
}

public class StatAdder : MonoBehaviour
{
    [SerializeField]
    AdderStatData[] adderStats;

    [SerializeField]
    StatPanel[] statPanels = new StatPanel[3];

    [SerializeField]
    GameObject UI;

    public bool isSelectingStat = false;

    public void AddStat(Stat addStat)
    {
        switch(addStat)
        {
            case Stat.HP:
                Player.instance.HpBonus += adderStats[(int)Stat.HP].value;
                Debug.Log(Player.instance.MaxHp);
                break;
            case Stat.Defence:
                Player.instance.DefenseBonus += adderStats[(int)Stat.Defence].value;
                Debug.Log(Player.instance.Defense);
                break;
            case Stat.AttackDamage:
                Player.instance.AttackDamageBonus += adderStats[(int)Stat.AttackDamage].value;
                Debug.Log(Player.instance.AttackDamage);
                break;
            case Stat.CriticalRange:
                Player.instance.CriticalRangeBonus += adderStats[(int)Stat.CriticalRange].value;
                Debug.Log(Player.instance.CriticalRange);
                break;
            case Stat.CriticalDamage:
                Player.instance.CriticalDamageBonus += adderStats[(int)Stat.CriticalDamage].value;
                Debug.Log(Player.instance.CriticalDamage);
                break;
            case Stat.Penetration:
                Player.instance.PenetrationBonus += adderStats[(int)Stat.Penetration].value;
                Debug.Log(Player.instance.Penetration);
                break;
            default:
                Debug.Log("Error");
                break;
        }

        OffUI();

        isSelectingStat = false;
    }

    public void SetStat()
    {
        isSelectingStat = true;

        List<AdderStatData> tempList = new List<AdderStatData>(adderStats);

        int index = UnityEngine.Random.Range(0, tempList.Count);
        AdderStatData data0 = tempList[index];
        tempList.RemoveAt(index);
        statPanels[0].Setup(data0);
        
        index = UnityEngine.Random.Range(0, tempList.Count);
        data0 = tempList[index];
        tempList.RemoveAt(index);
        statPanels[1].Setup(data0);

        index = UnityEngine.Random.Range(0, tempList.Count);
        data0 = tempList[index];
        tempList.RemoveAt(index);
        statPanels[2].Setup(data0);

        OnUI();
    }

    void OnUI()
    {
        UI.SetActive(true);
    }

    void OffUI()
    {
        UI.SetActive(false);
    }
}
