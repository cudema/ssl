using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EffectItemKeyword
{
    None = 0,
    Conflict
}

public class EffectItem : MonoBehaviour, IPointerClickHandler
{

    [SerializeReference, SubclassSelector]
    IEffect effect;

    public int effectID;

    public EffectItemKeyword keyword;

    public EffectItem keywordItem;

    [SerializeField]
    string effectName;
    [SerializeField, Multiline(5)]
    string effectDescription;

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.instance.ChangeEffectName(effectName, effectDescription);
    }

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
