using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    DropSlot[] slots;

    InventoryManager inventoryManager = InventoryManager.instance;

    void Awake()
    {
        slots = GetComponentsInChildren<DropSlot>();
    }

    
}
