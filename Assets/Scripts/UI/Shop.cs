using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : UIBase
{    
    [SerializeField]
    EffectItem[] effectItems = new EffectItem[3];
    [SerializeField]
    Text[] texts;
    Toggle[] toggles;

    int currentSelrectedIndex = -1;
    RarityRange currentRarityrange;
    string currentRarity;

    bool isHaveItem = false;

    [SerializeField]
    TextMeshProUGUI text;


    void Awake()
    {
        toggles = UI.GetComponentsInChildren<Toggle>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && UI.activeSelf)
        {
            OffUI();
        }
    }

    public void OnShop(RarityRange rarityRange)
    {
        OnUI();

        if (isHaveItem) return;

        isHaveItem = true;

        toggles[0].interactable = true;
        toggles[1].interactable = true;
        toggles[2].interactable = true;
        currentSelrectedIndex = -1;
        currentRarityrange = rarityRange;
        int tempRange = Random.Range(0, 100);
        string rarity = tempRange switch
        {
            var x when x < rarityRange.NomalRange                                                       => "Nomal",
            var x when x < rarityRange.NomalRange + rarityRange.RareRange                               => "Rare",
            var x when x < rarityRange.NomalRange + rarityRange.RareRange + rarityRange.LegendaryRange  => "Legendary",
            _ => null
        };

        currentRarity = rarity;

        if (rarity == null)
        {
            Debug.LogError("Miss to rarity range selrect");
            return;
        }

        string tempPath = "EffectItem/" + currentRarity;
        List<EffectItem> loadEffectItems = new List<EffectItem>();
        loadEffectItems.AddRange(Resources.LoadAll<EffectItem>(tempPath));



        foreach (EffectItem item in loadEffectItems)
        {
            if (InventoryManager.instance.ChackHaveEffect(item))
            {
                loadEffectItems.Remove(item);
                if (item.keyword == EffectItemKeyword.Conflict)
                {
                    loadEffectItems.Remove(item.keywordItem);
                }
            }
        }

        int randomItem = Random.Range(0, loadEffectItems.Count);

        effectItems[0] = loadEffectItems[randomItem];
        loadEffectItems.RemoveAt(randomItem);
        texts[0].text = effectItems[0].effectName;


        if (effectItems[0].keyword == EffectItemKeyword.None)
        {
            randomItem = Random.Range(0, loadEffectItems.Count);

            effectItems[1] = loadEffectItems[randomItem];
            loadEffectItems.RemoveAt(randomItem);
            texts[1].text = effectItems[1].effectName;

            randomItem = Random.Range(0, loadEffectItems.Count);

            effectItems[2] = loadEffectItems[randomItem];
            loadEffectItems.RemoveAt(randomItem);
            texts[2].text = effectItems[2].effectName;
            
            return;
        }
    }

    public void Reset()
    {
        isHaveItem = false;
        
    }

    [SerializeField, Header("가격 설정")]
    int nomalPrice = 15;
    [SerializeField]
    int rarePrice = 30;
    [SerializeField]
    int legendaryPrice = 60;

    public void OnSelrectEffect()
    {
        if (currentSelrectedIndex == -1)
        {
            return;
        }
        int useCoin = 9999999;

        switch (currentRarity)
        {
            case "Nomal":
                useCoin = nomalPrice;
                break;
            case "Rare":
                useCoin = rarePrice;
                break;
            case "Legendary":
                useCoin = legendaryPrice;
                break;
        }

        if (!EconomyManager.Instance.TrySpendGold(useCoin))
        {
            OnErrerText();
            return;
        }

        EffectItem temp = Instantiate(effectItems[currentSelrectedIndex]);
        toggles[currentSelrectedIndex].interactable = false;
        currentSelrectedIndex = -1;
        UIManager.instance.inventory.AddItem(temp);

        //OffUI();
    }

    [SerializeField, Header("리롤 설정")]
    float rerollCoin = 15;
    [SerializeField]
    float rerollCoinAdder = 1.5f;

    public void OnReroll()
    {
        if (!EconomyManager.Instance.TrySpendGold((int)rerollCoin))
        {
            OnErrerText();
            return;
        }
        rerollCoin *= rerollCoinAdder;
        toggles[0].interactable = true;
        toggles[1].interactable = true;
        toggles[2].interactable = true;

        int tempRange = Random.Range(0, 100);
        string rarity = tempRange switch
        {
            var x when x < currentRarityrange.NomalRange                                                                    => "Nomal",
            var x when x < currentRarityrange.NomalRange + currentRarityrange.RareRange                                     => "Rare",
            var x when x < currentRarityrange.NomalRange + currentRarityrange.RareRange + currentRarityrange.LegendaryRange => "Legendary",
            _ => null
        };
        
        currentRarity = rarity;

        if (rarity == null)
        {
            Debug.LogError("Miss to rarity range selrect");
            return;
        }

        string tempPath = "EffectItem/" + currentRarity;
        List<EffectItem> loadEffectItems = new List<EffectItem>();
        loadEffectItems.AddRange(Resources.LoadAll<EffectItem>(tempPath));



        foreach (EffectItem item in loadEffectItems)
        {
            if (InventoryManager.instance.ChackHaveEffect(item))
            {
                loadEffectItems.Remove(item);
                if (item.keyword == EffectItemKeyword.Conflict)
                {
                    loadEffectItems.Remove(item.keywordItem);
                }
            }
        }

        int randomItem = Random.Range(0, loadEffectItems.Count);

        effectItems[0] = loadEffectItems[randomItem];
        loadEffectItems.RemoveAt(randomItem);
        texts[0].text = effectItems[0].name;


        if (effectItems[0].keyword == EffectItemKeyword.None)
        {
            randomItem = Random.Range(0, loadEffectItems.Count);

            effectItems[1] = loadEffectItems[randomItem];
            loadEffectItems.RemoveAt(randomItem);
            texts[1].text = effectItems[1].name;

            randomItem = Random.Range(0, loadEffectItems.Count);

            effectItems[2] = loadEffectItems[randomItem];
            loadEffectItems.RemoveAt(randomItem);
            texts[2].text = effectItems[2].name;
            
            return;
        }
    }

    public void SetIndex(int selrectIndex)
    {
        currentSelrectedIndex = selrectIndex;
        text.transform.parent.gameObject.SetActive(true);
        text.text = effectItems[currentSelrectedIndex].effectDescription;
    }

    public override void OnUI()
    {
        base.OnUI();
        Player.instance.StopPlayer();
    }

    public override void OffUI()
    {
        text.transform.parent.gameObject.SetActive(false);
        base.OffUI();
        currentSelrectedIndex = -1;
        Player.instance.SetupPlayer();
        InputManager.instance.StartControll();
    }

    [SerializeField, Header("오류 메세지")]
    GameObject errerText;
    [SerializeField]
    float errerTextPrintTime = 1f;

    Coroutine printErrerText;

    void OnErrerText()
    {
        if (printErrerText != null) StopCoroutine(printErrerText);
        printErrerText = StartCoroutine(PrintErrerText());
    }

    IEnumerator PrintErrerText()
    {
        errerText.SetActive(true);
        yield return new WaitForSecondsRealtime(errerTextPrintTime);
        errerText.SetActive(false);
    }
}
