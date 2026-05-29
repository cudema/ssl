using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : InteractiveObject
{
    [SerializeField]
    float value;

    public void SetValue(float value)
    {
        this.value = value;
    }

    protected override void OnAction()
    {
        Player.instance.CurrentHp += Player.instance.MaxHp * value;
        isInteractiable = false;
    }
}
