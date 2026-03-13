using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : InteractiveObject
{
    protected override void OnAction()
    {
        Player.instance.CurrentHp += Player.instance.MaxHp * 0.7f;
    }
}
