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

    void Start()
    {
        InputManager.instance.move.performed += movement.ToPlayerMove;
        InputManager.instance.move.canceled += movement.ToStap;
        InputManager.instance.cameraAngle.performed += movement.ToMoveCameraAngle;
        InputManager.instance.cameraAngle.canceled += movement.CancelCameraAngle;
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        InputManager.instance.move.performed -= movement.ToPlayerMove;
        InputManager.instance.move.canceled -= movement.ToStap;
        InputManager.instance.cameraAngle.performed -= movement.ToMoveCameraAngle;
        InputManager.instance.cameraAngle.canceled -= movement.CancelCameraAngle;
    }
}
