using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EffectItemKeyword
{
    None = 0,
    Conflict
}

[System.Serializable]
public enum WeaponType
{
    All = 0,
    Sword,
    Axe,
    Spear
}

public class EffectItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    public WeaponType weapon;

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
        if (weapon == WeaponType.All)
        {
            Player.instance.playerEffectHandler.AddEffect(effect);
            return;
        }
        OnChackWeapon();
        Player.instance.playerWeapon.ChangedWeapon += OnChackWeapon;
    }

    public void OnRemoveEffect()
    {
        Player.instance.playerEffectHandler.RemoveEffect(effect);
        Player.instance.playerWeapon.ChangedWeapon -= OnChackWeapon;
    }

    public void OnChackWeapon()
    {
        if (weapon == Player.instance.playerWeapon.currentWeapon.weaponType)
        {
            Player.instance.playerEffectHandler.AddEffect(effect);
            return;
        }

        Player.instance.playerEffectHandler.RemoveEffect(effect);
    }
}