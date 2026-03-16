using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{    
    [SerializeField]
    GameObject UI;

    [SerializeField]
    EffectItem[] effectItems = new EffectItem[3];
    [SerializeField]
    Text[] texts;
    Toggle[] toggles;

    int currentSelrectedIndex = -1;
    RarityRange currentRarityrange;
    string currentRarity;

    void Awake()
    {
        toggles = UI.GetComponentsInChildren<Toggle>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffUI();
        }
    }

    public void OnShop(RarityRange rarityRange)
    {
        OnUI();
        toggles[0].interactable = true;
        toggles[1].interactable = true;
        toggles[2].interactable = true;
        currentSelrectedIndex = -1;
        currentRarityrange = rarityRange;
        float tempRange = Random.Range(0f, 1f);
        string rarity = tempRange switch
        {
            var x when x < rarityRange.NomalRange       => "Nomal",
            var x when x < rarityRange.RareRange        => "Rare",
            var x when x < rarityRange.LegendaryRange   => "Legendary",
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
                useCoin = 15;
                break;
            case "Rare":
                useCoin = 30;
                break;
            case "Legendary":
                useCoin = 60;
                break;
        }

        if (!EconomyManager.Instance.TrySpendGold(useCoin))
        {
            return;
        }

        EffectItem temp = Instantiate(effectItems[currentSelrectedIndex]);
        toggles[currentSelrectedIndex].interactable = false;
        currentSelrectedIndex = -1;
        UIManager.instance.inventory.AddItem(temp);

        //OffUI();
    }
    
    public void OnReroll()
    {
        if (!EconomyManager.Instance.TrySpendGold(15))
        {
            return;
        }

        toggles[0].interactable = true;
        toggles[1].interactable = true;
        toggles[2].interactable = true;

        float tempRange = Random.Range(0f, 1f);
        string rarity = tempRange switch
        {
            var x when x < currentRarityrange.NomalRange       => "Nomal",
            var x when x < currentRarityrange.RareRange        => "Rare",
            var x when x < currentRarityrange.LegendaryRange   => "Legendary",
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
    }

    public void OnUI()
    {
        UI.SetActive(true);
        Player.instance.StopPlayer();
    }

    public void OffUI()
    {
        UI.SetActive(false);
        Player.instance.SetupPlayer();
    }
}
