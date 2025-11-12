using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    PlayerInput input;
    InputActionMap playerMoveMap;

    public InputAction move;
    public InputAction cameraAngle;
    public InputAction attack;
    public InputAction changeWeapon;
    public InputAction skill;
    public InputAction desh;

    InputActionMap currentActinoMap;

    void Awake()
    {
        Application.targetFrameRate = 120;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        input = GetComponent<PlayerInput>();

        playerMoveMap = input.actions.FindActionMap("Move");
        if (playerMoveMap != null)
        {
            move = playerMoveMap.FindAction("move");
            cameraAngle = playerMoveMap.FindAction("cameraAngle");
            attack = playerMoveMap.FindAction("Attack");
            changeWeapon = playerMoveMap.FindAction("ChangeWeapon");
            skill = playerMoveMap.FindAction("Skill");
            desh = playerMoveMap.FindAction("Desh");
        }
    }

    void Start()
    {
        playerMoveMap.Enable();
    }
}
