using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactive
{
    public void OnInteraction();
}

public class InteractiveObject : MonoBehaviour, Interactive
{
    [SerializeField]
    bool singleUse;

    bool isInteractiable = true;
    public bool IsInteractiable
    {
        get => isInteractiable;
    }

    public void OnInteraction()
    {
        if (isInteractiable || !singleUse)
        {
            isInteractiable = false;
            OnAction();
        }
    }

    protected virtual void OnAction()
    {
        
    }
}
