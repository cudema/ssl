using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UpgradeData
{
    public int Level;
    public int Gold;
    public int AttackDamage;
    public float CriticalRange;
}

[CreateAssetMenu(fileName = "WeaponUpgradeTable", menuName = "WeaponUpgradeTable/Table")]
public class WeaponUpgradeTable : ScriptableObject
{
    public List<UpgradeData> table;
}
