using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class EffectAdder : MonoBehaviour
{
    [SerializeField]
    GameObject UI;

    [SerializeField]
    EffectItem[] effectItems = new EffectItem[3];

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
        Debug.Log(rarityRange.NomalRange);
        if (rarity == null)
        {
            Debug.LogError("Miss to rarity range selrect");
            return;
        }

        string tempPath = "EffectItem/" + rarity;
        List<EffectItem> loadEffectItems = new List<EffectItem>();
        loadEffectItems.AddRange(Resources.LoadAll<EffectItem>(tempPath));

        int randomItem = Random.Range(0, loadEffectItems.Count);

        effectItems[0] = loadEffectItems[randomItem];

        
    }

    public void OnSelrectEffect()
    {
        if (currentSelrectedIndex == -1)
        {
            return;
        }

        EffectItem temp = Instantiate(effectItems[currentSelrectedIndex]);
        UIManager.instance.inventory.AddItem(temp);

        OffUI();
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
