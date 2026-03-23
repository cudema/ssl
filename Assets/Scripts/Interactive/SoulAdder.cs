using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulAdder : InteractiveObject
{
    protected override void OnAction()
    {
        UIManager.instance.soul.OnUI();
    }
}