using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 5;
    [SerializeField]
    float rotationSpeed = 10;
    [SerializeField]
    float angleSpeed = 10;
    [SerializeField]
    float angleLockTime = 3;
    [SerializeField]
    Transform cameraAngle;

    float angleX;
    float angleY;

    bool playerMoveable;

    Movement movement;

    IEnumerator angleMoveCorutine;

    Vector3 playerMoveDirection;
    public Vector3 PlayerDirection
    {
        get => dir;
    }

    Vector3 dir;

    void Awake()
    {
        movement = GetComponent<Movement>();
        angleX = cameraAngle.rotation.x;
        angleY = cameraAngle.rotation.y;
        angleMoveCorutine = ToAngle();
    }

    void Start()
    {
        movement.SetSpeed(speed, rotationSpeed);
    }

    void Update()
    {
        dir = Quaternion.AngleAxis(cameraAngle.localEulerAngles.y, transform.up) * playerMoveDirection;
        if (dir != Vector3.zero)
        {
            movement.ToMove(dir);
        }
    }

    public void ToPlayerMove(InputAction.CallbackContext value)
    {
        Vector2 tempVector = value.ReadValue<Vector2>();
        playerMoveDirection = new Vector3(tempVector.x, 0, tempVector.y);
        playerMoveable = true;
    }

    public void ToStap(InputAction.CallbackContext value)
    {
        playerMoveable = false;
        
        playerMoveDirection = Vector3.zero;
    }

    public void ToMoveCameraAngle(InputAction.CallbackContext value)
    {
        StopCoroutine(angleMoveCorutine);

        Vector2 tempVector = value.ReadValue<Vector2>();
        angleX -= tempVector.y * Time.deltaTime * angleSpeed;
        angleY += tempVector.x * Time.deltaTime * angleSpeed;

        ChackAngleX();

        cameraAngle.rotation = Quaternion.Euler(new Vector3(angleX, angleY, 0));
    }

    public void CancelCameraAngle(InputAction.CallbackContext value)
    {
        StopCoroutine(angleMoveCorutine);
        angleMoveCorutine = ToAngle();
        StartCoroutine(angleMoveCorutine);
    }

    IEnumerator ToAngle()
    {
        yield return new WaitUntil(() => playerMoveable);
        yield return new WaitForSeconds(angleLockTime);

        while (true)
        {
            if (Mathf.Abs(((cameraAngle.forward - new Vector3(0, cameraAngle.forward.y, 0)).normalized + movement.GetTransform().forward).x) < 0.4f && Mathf.Abs(((cameraAngle.forward - new Vector3(0, cameraAngle.forward.y, 0)).normalized + movement.GetTransform().forward).z) < 0.4f)
            {
                yield return null;
                continue;
            }
            angleX = cameraAngle.localEulerAngles.x;
            angleY = cameraAngle.localEulerAngles.y;
            Quaternion tempDir = Quaternion.LookRotation(movement.GetTransform().forward + new Vector3(angleX, 0, 0));
            cameraAngle.rotation = Quaternion.Lerp(cameraAngle.rotation, tempDir, Time.deltaTime * angleSpeed / 5);
            yield return null;
        }
    }

    void ChackAngleX()
    {
        if (angleX < -17)
        {
            angleX = -17;
            return;
        }
        if (angleX > 75)
        {
            angleX = 75;
            return;
        }
    }
}
