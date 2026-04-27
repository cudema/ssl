using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    DropSlot[] slots;

    EffectItem[] items;

    [SerializeField]
    GameObject panel;

    public int activeItemSlot;

    [SerializeField]
    TextMeshProUGUI effectName;
    [SerializeField]
    TextMeshProUGUI effectDescription;

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

        slots = panel.GetComponentsInChildren<DropSlot>();
        items = new EffectItem[slots.Length];
        OffText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (panel.activeSelf)
            {
                OffUI();
            }
            else
            {
                OnUI();
            }
        }
    }

    public void AddItem(EffectItem item)
    {
        int index = 5;

        for (int i = index; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                index = i;
                break;
            }
        }

        DragAndDrop tempDAD = item.GetComponent<DragAndDrop>();
        tempDAD.index = index;
        items[index] = item;
        item.transform.SetParent(slots[index].transform);
        tempDAD.Setup();
        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        ChangeSlot(-1, index);

        // item.transform.SetParent(spownPoint);
        // DragAndDrop tempDAD = item.GetComponent<DragAndDrop>();
        // tempDAD.index = -1;
        // tempDAD.Setup();
        // items[slots.Length - 1] = item;
        // item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void ChangeSlot(int beforeIndex, int afterIndex)
    {
        if (beforeIndex == -1)
        {
            if (afterIndex < activeItemSlot)
            {
                items[afterIndex].OnAddEffect();
            }
            return;
        }

        EffectItem beforeItem = items[beforeIndex];
        EffectItem afterItem = items[afterIndex];
        items[beforeIndex] = afterItem;
        items[afterIndex] = beforeItem;

        // Debug.Log($"index 0: {items[0]}");
        // Debug.Log($"index 1: {items[1]}");
        // Debug.Log($"index 2: {items[2]}");
        // Debug.Log($"index 3: {items[3]}");
        // Debug.Log($"index 4: {items[4]}");
        // Debug.Log($"index 5: {items[5]}");
        // Debug.Log($"index 6: {items[6]}");
        // Debug.Log($"index 7: {items[7]}");

        if (beforeIndex < activeItemSlot && afterIndex < activeItemSlot)
        {
            return;
        }

        if (beforeIndex < activeItemSlot)
        {
            beforeItem?.OnRemoveEffect();
            afterItem?.OnAddEffect();
        }

        if (afterIndex < activeItemSlot)
        {
            beforeItem?.OnAddEffect();
            afterItem?.OnRemoveEffect();
        }

        Debug.Log($"Before item: {beforeItem}");
        Debug.Log($"After item: {afterItem}");
    }

    public void OnUI()
    {
        Player.instance.StopPlayer();
        panel.SetActive(true);
    }

    public void OffUI()
    {
        Player.instance.SetupPlayer();
        panel.SetActive(false);
    }

    public bool ChackHaveEffect(EffectItem chackItem)
    {
        foreach (EffectItem item in items)
        {
            if (item != null && chackItem.effectID == item.effectID)
            {
                return true;
            }
        }

        return false;
    }

    public void ChangeEffectName(string name, string description)
    {
        effectName.gameObject.SetActive(true);
        effectDescription.gameObject.SetActive(true);
        effectName.text = name;
        effectDescription.text = description;
    }

    public void OffText()
    {
        effectName.gameObject.SetActive(false);
        effectDescription.gameObject.SetActive(false);
    }
}
