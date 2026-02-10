using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Poison", menuName = "Buff System/Poison")]
public class Poison : BuffData
{
    [Header("Poison Value")]
    public float damageValue;

    public override void TickBuffEffect(IHealthable healthable)
    {
        healthable.OnTureDamage(value * damageValue);
    }
}
