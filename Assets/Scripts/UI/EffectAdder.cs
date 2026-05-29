using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct RarityRange
{
    [SerializeField]
    int nomalRange;
    [SerializeField]
    int rareRange;
    [SerializeField]
    int legendaryRange;

    public int NomalRange
    {
        get => nomalRange;
    }
    public int RareRange
    {
        get => rareRange;
    }
    public int LegendaryRange
    {
        get => legendaryRange;
    }
}

public class EffectAdder : UIBase
{
    [SerializeField]
    EffectItem[] effectItems = new EffectItem[3];
    [SerializeField]
    Text[] texts;
    [SerializeField]
    GameObject selrecter2;

    [SerializeField]
    ToggleGroup toggleGroup;
    [SerializeField]
    TextMeshProUGUI text;

    int currentSelrectedIndex = -1;

    public Action AddedEffect;

    public void SetEffect(RarityRange rarityRange)
    {
        OnUI();
        currentSelrectedIndex = -1;
        int tempRange = UnityEngine.Random.Range(0, 100);
        string rarity = tempRange switch
        {
            var x when x < rarityRange.NomalRange                                                      => "Nomal",
            var x when x < rarityRange.NomalRange + rarityRange.RareRange                              => "Rare",
            var x when x < rarityRange.NomalRange + rarityRange.RareRange + rarityRange.LegendaryRange => "Legendary",
            _ => null
        };
        //Debug.Log(tempRange);
        if (rarity == null)
        {
            Debug.LogError("Miss to rarity range selrect");
            return;
        }

        string tempPath = "EffectItem/" + rarity;
        EffectItem[] loadEffectItems = Resources.LoadAll<EffectItem>(tempPath);
        List<EffectItem> effectItems = new List<EffectItem>();

        foreach (EffectItem item in loadEffectItems)
        {
            if (InventoryManager.instance.ChackHaveEffect(item))
            {
                continue;
            }
            //Debug.Log(effectItems.Count);
            effectItems.Add(item);
        }

        int randomItem = UnityEngine.Random.Range(0, effectItems.Count);

        this.effectItems[0] = effectItems[randomItem];
        effectItems.RemoveAt(randomItem);
        texts[0].text = this.effectItems[0].effectName;


        if (effectItems[0].keyword == EffectItemKeyword.None)
        {
            randomItem = UnityEngine.Random.Range(0, effectItems.Count);

            this.effectItems[1] = effectItems[randomItem];
            effectItems.RemoveAt(randomItem);
            texts[1].text = this.effectItems[1].effectName;

            randomItem = UnityEngine.Random.Range(0, effectItems.Count);

            this.effectItems[2] = effectItems[randomItem];
            effectItems.RemoveAt(randomItem);
            texts[2].text = this.effectItems[2].effectName;

            selrecter2.SetActive(true);

            return;
        }

        this.effectItems[1] = this.effectItems[0].keywordItem;

        selrecter2.SetActive(false);
    }

    public void OnSelrectEffect()
    {
        if (currentSelrectedIndex == -1)
        {
            return;
        }

        EffectItem temp = Instantiate(effectItems[currentSelrectedIndex], UIManager.instance.transform);
        UIManager.instance.inventory.AddItem(temp);

        OffUI();
        AddedEffect?.Invoke();
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
        if (currentSelrectedIndex != -1)
        {
            toggleGroup.GetFirstActiveToggle().isOn = false;
        }
    }

    public override void OffUI()
    {
        text.transform.parent.gameObject.SetActive(false);
        base.OffUI();
        currentSelrectedIndex = -1;
        Player.instance.SetupPlayer();
        InputManager.instance.StartControll();
    }
}
