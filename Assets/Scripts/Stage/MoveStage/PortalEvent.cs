using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEvent : MonoBehaviour
{
    InteractiveObject inter;
    [SerializeField]
    Weapon weapon;

    void Awake()
    {
        inter = GetComponent<InteractiveObject>();
    }

    void OnEnable()
    {
        inter.OnInteractionEvent += PrintText;
    }

    void OnDisable()
    {
        inter.OnInteractionEvent -= PrintText;
    }

    void PrintText()
    {
        InventoryManager.instance.ResetInventory();
        Player.instance.OnPlayerStatReset();
        Player.instance.SetupWeapon(weapon, weapon);
    }
}
