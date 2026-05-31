using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public Setting setting;

    public WeaponUI weaponUI;
    public Pause pause;

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

    void Update()
    {
        if (SceneManager.GetActiveScene().name == SceneName.StartMenu.ToString()) return;

        if (gameMenuUI.UI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            gameMenuUI.OffUI();
            if (gameMenuUI.menuToggle.isOn)
            {
                gameMenuUI.menuToggle.isOn = false;
                gameMenuUI.currentIndex = 0;
                return;
            }
            if (gameMenuUI.inventoryToggle.isOn)
            {
                gameMenuUI.inventoryToggle.isOn = false;
                gameMenuUI.currentIndex = 1;
                return;
            }
            if (gameMenuUI.mapToggle.isOn)
            {
                gameMenuUI.mapToggle.isOn = false;
                gameMenuUI.currentIndex = 2;
                return;
            }
            // else
            // {
            //     OnUI();
            // }
        }
        else if (pause.isOnable && !pause.UI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            pause.OnUI();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.OffUI();
        }
    }
}
