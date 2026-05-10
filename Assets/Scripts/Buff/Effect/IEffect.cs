using System;
using UnityEngine;

[System.Serializable]
public abstract class Effect
{
    [SubclassSelector, SerializeReference]
    public BaseEffect useEffect;

    public abstract void OnApply(Player player);
    public abstract void OnRemove(Player player);
}

public abstract class UseEffect : Effect
{
    public abstract void OnEffect(BuffManager enemy);
}

public abstract class AddValueEffect : Effect
{
    public abstract float OnEffect(BuffManager enemy);
}