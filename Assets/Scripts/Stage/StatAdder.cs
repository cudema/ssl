using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    HP = 0,
    Defence,
    AttackDamage,
    Speed,
    CriticalRange,
    CriticalDamage,
    Penetration
}

public class StatAdder : MonoBehaviour
{
    public void SetStat(Stat addStat)
    {
        switch(addStat)
        {
            case Stat.HP:
                Player.instance.HpBonus += 50;
                break;
            case Stat.Defence:
                Player.instance.DefenseBonus += 5;
                break;
            case Stat.AttackDamage:
                Player.instance.AttackDamageBonus += 10;
                break;
            case Stat.Speed:
                Player.instance.SpeedBonus += 1;
                break;
            case Stat.CriticalRange:
                Player.instance.CriticalRangeBonus += 0.05f;
                break;
            case Stat.CriticalDamage:
                Player.instance.CriticalDamageBonus += 0.1f;
                break;
            case Stat.Penetration:
                Player.instance.PenetrationBonus += 5;
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }
}
