using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed;

    Movement movement;

    Vector3 playerMoveDirection;

    void Awake()
    {
        movement = GetComponent<Movement>();
    }

    void Start()
    {
        movement.SetSpeed(speed);
    }

    void Update()
    {
        movement.ToMove(playerMoveDirection);
    }

    public void ToPlayerMove(InputAction.CallbackContext value)
    {
        Vector2 tempVector = value.ReadValue<Vector2>();
        playerMoveDirection = new Vector3(tempVector.x, 0, tempVector.y);
    }
}
