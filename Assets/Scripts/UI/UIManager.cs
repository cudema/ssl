using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static public UIManager instance;

    public WeaponIcon weaponIcon;
    public CollDown mainSkillColldown;
    public CollDown subSkillColldown;
    public CollDown dechCollDown;
    public CollDown SwitchingColldown;
    public StatAdder statAdder;
    public HpBar hpBar;
    public InventoryManager inventory;
    public EffectAdder effectAdder;
    public Shop shop;
    public SoulUI soul;
    public WeaponSelrectUI weaponSelrect;
    public GameMenuUI gameMenuUI;
    public UIBase BattleUI;

    public WeaponUI weaponUI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
