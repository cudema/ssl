using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    Weapon currentWeapon;

    Weapon mainWeapon;
    Weapon subWeapon;

    void Awake()
    {
        mainWeapon = new NoWeapon(1.0f);
        subWeapon = new NoWeapon(1.0f);
    }

    void Start()
    {
        currentWeapon = mainWeapon;
    }

    public void ChangeWeapon(InputAction.CallbackContext value)
    {
        currentWeapon?.UnequipWeapon();

        if (currentWeapon == mainWeapon)
        {
            currentWeapon = subWeapon;
        }
        else
        {
            currentWeapon = mainWeapon;
        }

        currentWeapon.EquipWeapon();
    }

    public void Attack(InputAction.CallbackContext value)
    {
        currentWeapon.AttackWeapon();
    }
}
