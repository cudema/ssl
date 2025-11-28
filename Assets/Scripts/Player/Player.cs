using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    PlayerMovement movement;
    PlayerWeapon playerWeapon;
    PlayerInputController playerInputController;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        movement = GetComponent<PlayerMovement>();
        playerWeapon = GetComponent<PlayerWeapon>();
        playerInputController = GetComponent<PlayerInputController>();
    }

    public void OnPositionSet(Vector3 vector)
    {
        transform.position = vector;
    }

    public void SetupPlayer()
    {
        movement.PlayerMoveable = true;
        InputManager.instance.StartControll();
    }

    public void StopPlayer()
    {
        movement.PlayerMoveable = false;
        InputManager.instance.StopControll();
    }

    public void SetupWeapon(Weapon mainWeapon, Weapon subWeapon)
    {
        playerWeapon.SetupWeapon(mainWeapon, subWeapon);
        playerInputController.Setup();
        SetupPlayer();
    }
}
