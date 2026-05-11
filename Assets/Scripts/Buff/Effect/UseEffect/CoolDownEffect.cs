using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddTypeMenu("스킬 쿨다운 감소")]
public class CoolDownEffect : BaseEffect
{
    [SerializeField, Range(0f, 1f)]
    float value;

    public override void OnEffect(BuffManager buffmanager)
    {
        Player.instance.playerWeapon.currentWeapon.ReduceCollDown(value);
    }
}
