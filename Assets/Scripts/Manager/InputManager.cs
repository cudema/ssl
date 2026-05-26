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
    public InputAction interaction;

    InputActionMap currentActinoMap;

    bool isInputable;

    void Awake()
    {
        Application.targetFrameRate = 120;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
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
            interaction = playerMoveMap.FindAction("Interaction");
        }
    }

    void Start()
    {
        //StartControll();
        //playerMoveMap.Enable();
    }

    public void StopControll()
    {
        isInputable = false;
        playerMoveMap.Disable();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Player.instance.movement.movement.Controller.enabled = false;
    }

    public void StartControll()
    {
        isInputable = true;
        playerMoveMap.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Player.instance.movement.movement.Controller.enabled = true;
    }

    public bool GetInputUseable()
    {
        return isInputable;
    }
}
