using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct RarityRange
{
    [SerializeField]
    float nomalRange;
    [SerializeField]
    float rareRange;
    [SerializeField]
    float legendaryRange;

    public float NomalRange
    {
        get => nomalRange;
    }
    public float RareRange
    {
        get => rareRange;
    }
    public float LegendaryRange
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

    public void SetEffect(RarityRange rarityRange)
    {
        OnUI();
        currentSelrectedIndex = -1;
        float tempRange = Random.Range(0f, 1f);
        string rarity = tempRange switch
        {
            var x when x < rarityRange.NomalRange       => "Nomal",
            var x when x < rarityRange.RareRange        => "Rare",
            var x when x < rarityRange.LegendaryRange   => "Legendary",
            _ => null
        };
        
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

        int randomItem = Random.Range(0, effectItems.Count);

        this.effectItems[0] = effectItems[randomItem];
        effectItems.RemoveAt(randomItem);
        texts[0].text = this.effectItems[0].effectName;


        if (effectItems[0].keyword == EffectItemKeyword.None)
        {
            randomItem = Random.Range(0, effectItems.Count);

            this.effectItems[1] = effectItems[randomItem];
            effectItems.RemoveAt(randomItem);
            texts[1].text = this.effectItems[1].effectName;

            randomItem = Random.Range(0, effectItems.Count);

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
    }

    public void SetIndex(int selrectIndex)
    {
        currentSelrectedIndex = selrectIndex;
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
        base.OffUI();
        currentSelrectedIndex = -1;
        Player.instance.SetupPlayer();
        InputManager.instance.StartControll();
    }
}
