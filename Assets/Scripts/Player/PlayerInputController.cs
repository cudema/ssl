using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    PlayerMovement movement;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void OnEnable()
    {
        InputManager.instance.move.performed += movement.ToPlayerMove;
        InputManager.instance.move.canceled += movement.ToPlayerMove;
    }

    void OnDisable()
    {
        InputManager.instance.move.performed -= movement.ToPlayerMove;
        InputManager.instance.move.canceled -= movement.ToPlayerMove;
    }
}
