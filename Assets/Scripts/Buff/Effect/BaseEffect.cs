using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect
{
    [SerializeField]
    float cooltime;

    float temptime;

    public virtual void OnApply()
    {
        
    }

    public virtual void OnRemove()
    {
        
    }

    public void OnUseEffect(BuffManager buffManager)
    {
        if (Time.time - temptime < cooltime)
        {
            return;
        }
        temptime = Time.time;
        OnEffect(buffManager);
    }

    public virtual void OnEffect(BuffManager buffmanager)
    {

    }
}
