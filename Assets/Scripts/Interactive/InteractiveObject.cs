using System;
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
    [SerializeField]
    GameObject effect;

    ParticleSystem particle;

    protected bool isInteractiable = true;
    public bool IsInteractiable
    {
        get => isInteractiable;
    }

    public Action OnInteractionEvent;

    void Start()
    {
        if (effect != null)
        {
            particle = Instantiate(effect, transform).GetComponent<ParticleSystem>();
        }
    }

    public void OnInteraction()
    {
        if (isInteractiable || !singleUse)
        {
            if (effect != null)
            {
                particle.Stop();
                particle.Clear();
            }
            OnAction();
        }
    }

    protected virtual void OnAction()
    {
        isInteractiable = false;
        OnInteractionEvent?.Invoke();
    }
}
