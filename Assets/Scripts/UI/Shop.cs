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
    [SerializeField]
    TextMeshProUGUI buyText;
    [SerializeField]
    TextMeshProUGUI rerollText;


    void Awake()
    {
        toggles = UI.GetComponentsInChildren<Toggle>();
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
        Debug.Log(tempRange);
        string rarity = tempRange switch
        {
            var x when x < rarityRange.NomalRange                                                       => "Nomal",
            var x when x < rarityRange.NomalRange + rarityRange.RareRange                               => "Rare",
            var x when x < rarityRange.NomalRange + rarityRange.RareRange + rarityRange.LegendaryRange  => "Legendary",
            _ => null
        };

        currentRarity = rarity;
        currentRerollCoin = rerollCoin;
        rerollText.text = ((int)currentRerollCoin).ToString();
        switch (currentRarity)
        {
            case "Nomal":
                useCoin = nomalPrice;
                buyText.text = nomalPrice.ToString();
                break;
            case "Rare":
                useCoin = rarePrice;
                buyText.text = rarePrice.ToString();
                break;
            case "Legendary":
                useCoin = legendaryPrice;
                buyText.text = legendaryPrice.ToString();
                break;
        }

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

    int useCoin = 9999999;
    public void OnSelrectEffect()
    {
        if (currentSelrectedIndex == -1)
        {
            return;
        }

        if (!EconomyManager.Instance.TrySpendGold(useCoin))
        {
            OnErrerText();
            return;
        }

        EffectItem temp = Instantiate(effectItems[currentSelrectedIndex], UIManager.instance.transform);
        toggles[currentSelrectedIndex].interactable = false;
        currentSelrectedIndex = -1;
        UIManager.instance.inventory.AddItem(temp);

        //OffUI();
    }

    [SerializeField, Header("리롤 설정")]
    float rerollCoin = 15;
    [SerializeField]
    float rerollCoinAdder = 1.5f;

    float currentRerollCoin;

    public void OnReroll()
    {
        if (!EconomyManager.Instance.TrySpendGold((int)currentRerollCoin))
        {
            OnErrerText();
            return;
        }
        currentRerollCoin *= rerollCoinAdder;
        rerollText.text = ((int)currentRerollCoin).ToString();
        toggles[0].interactable = true;
        toggles[1].interactable = true;
        toggles[2].interactable = true;

        int tempRange = Random.Range(0, 100);
        Debug.Log(tempRange);
        string rarity = tempRange switch
        {
            var x when x < currentRarityrange.NomalRange                                                                    => "Nomal",
            var x when x < currentRarityrange.NomalRange + currentRarityrange.RareRange                                     => "Rare",
            var x when x < currentRarityrange.NomalRange + currentRarityrange.RareRange + currentRarityrange.LegendaryRange => "Legendary",
            _ => null
        };

        if (rarity == null)
        {
            Debug.LogError("Miss to rarity range selrect");
            return;
        }
        currentRarity = rarity;

        switch (currentRarity)
        {
            case "Nomal":
                useCoin = nomalPrice;
                buyText.text = nomalPrice.ToString();
                break;
            case "Rare":
                useCoin = rarePrice;
                buyText.text = rarePrice.ToString();
                break;
            case "Legendary":
                useCoin = legendaryPrice;
                buyText.text = legendaryPrice.ToString();
                break;
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
