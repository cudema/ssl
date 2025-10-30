using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    PlayerMovement movement;
    PlayerWeapon playerWeapon;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        playerWeapon = GetComponent<PlayerWeapon>();
    }

    void Start()
    {
        InputManager.instance.move.performed += movement.ToPlayerMove;
        InputManager.instance.move.canceled += movement.ToStap;
        InputManager.instance.cameraAngle.performed += movement.ToMoveCameraAngle;
        //InputManager.instance.cameraAngle.canceled += movement.CancelCameraAngle;
        InputManager.instance.attack.performed += playerWeapon.Attack;
        InputManager.instance.changeWeapon.performed += playerWeapon.ChangeWeapon;
        InputManager.instance.skill.performed += playerWeapon.Skill;
        InputManager.instance.desh.performed += playerWeapon.Desh;
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        InputManager.instance.move.performed -= movement.ToPlayerMove;
        InputManager.instance.move.canceled -= movement.ToStap;
        InputManager.instance.cameraAngle.performed -= movement.ToMoveCameraAngle;
        //InputManager.instance.cameraAngle.canceled -= movement.CancelCameraAngle;
        InputManager.instance.attack.performed -= playerWeapon.Attack;
        InputManager.instance.changeWeapon.performed -= playerWeapon.ChangeWeapon;
        InputManager.instance.skill.performed -= playerWeapon.Skill;
        InputManager.instance.desh.performed -= playerWeapon.Desh;
    }
}
