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

    bool isInteractiable = true;
    public bool IsInteractiable
    {
        get => isInteractiable;
    }

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
            isInteractiable = false;
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
        
    }
}
