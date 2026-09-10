using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulAdder : InteractiveAction
{
    public override void OnAction()
    {
        UIManager.instance.soul.OnUI();
    }
}