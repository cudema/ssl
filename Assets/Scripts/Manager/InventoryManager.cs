using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    [SerializeField]
    DropSlot[] slots;

    [SerializeField]
    EffectItem[] items;

    [SerializeField]
    GameObject panel;

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
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        int index = 0;

        for (int i = 0; i < items.Length; i++)
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
    }

    public void ChangeSlot(int beforeIndex, int afterIndex)
    {
        if (beforeIndex == -1)
        {
            if (afterIndex < 6)
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

        if (beforeIndex < 6 && afterIndex < 6)
        {
            return;
        }

        if (beforeIndex < 6)
        {
            beforeItem?.OnRemoveEffect();
            afterItem?.OnAddEffect();
        }

        if (afterIndex < 6)
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
}
