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

    InputActionMap currentActinoMap;

    void Awake()
    {
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
        }
    }

    void Start()
    {
        playerMoveMap.Enable();
    }
}
