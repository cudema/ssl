using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "WeaponData/AttackRangeData")]
public class AttackRangeData : ScriptableObject
{
    public float moveDist;
    public float actionTime;
    public bool passThrough;
}

