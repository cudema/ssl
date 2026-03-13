using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectItemKeyword
{
    None = 0,
    Conflict
}

public class EffectItem : MonoBehaviour
{

    [SerializeReference, SubclassSelector]
    IEffect effect;

    public EffectItemKeyword keyword;

    public EffectItem keywordItem;

    void Start()
    {
        //InventoryManager.instance.AddItem(this, 6);
    }

    public void OnAddEffect()
    {
        Player.instance.playerEffectHandler.AddEffect(effect);
    }

    public void OnRemoveEffect()
    {
        Player.instance.playerEffectHandler.RemoveEffect(effect);
    }
}
