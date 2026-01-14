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
    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    float maxCameraDistance;

    public float MaxCameraDistance
    {
        get => maxCameraDistance;
    }

    float angleX;
    float angleY;

    bool playerMoviing;

    public Movement movement;
    Rigidbody playerRigidbody;

    IEnumerator angleMoveCorutine;
    Animator animator;
    Vector3 playerMoveDirection;
    public Vector3 PlayerDirection
    {
        get => dir;
    }

    public bool PlayerMoveable = true;

    Vector3 dir;

    void Awake()
    {
        movement = GetComponent<Movement>();
        angleX = cameraAngle.rotation.x;
        angleY = cameraAngle.rotation.y;
        angleMoveCorutine = ToAngle();
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        movement.SetSpeed(speed, rotationSpeed);
    }

    void Update()
    {
        dir = Quaternion.AngleAxis(cameraAngle.localEulerAngles.y, transform.up) * playerMoveDirection;
        if (PlayerMoveable && dir != Vector3.zero)
        {
            movement.ToMove(dir);
        }

        animator.SetFloat("Speed", Vector3.Distance(Vector3.zero, movement.Controller.velocity));

        if (InputManager.instance.GetInputUseable())
        {
            movement.OnGravity();
        }

    }

    public void ToPlayerMove(InputAction.CallbackContext value)
    {
        Vector2 tempVector = value.ReadValue<Vector2>();
        playerMoveDirection = new Vector3(tempVector.x, 0, tempVector.y);
        playerMoviing = true;
        animator.SetBool("IsMove", playerMoviing);
    }

    public void ToStap(InputAction.CallbackContext value)
    {
        playerMoviing = false;
        
        animator.SetBool("IsMove", playerMoviing);
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
        yield return new WaitUntil(() => playerMoviing);
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

    public void MoveCamaraDistance(float distance)
    {
        if (distance > maxCameraDistance)
        {
            return;
        }

        cameraTransform.localPosition = new Vector3(0, 0, -distance);
    }

    public void CameraSet()
    {
        angleX = 0;
        angleY = 0;
        cameraAngle.rotation = Quaternion.Euler(new Vector3(angleX, angleY, 0));
        movement.FastLookAt(new Vector3(0, 0, 1));
    }

    void ChackAngleX()
    {
        if (angleX < 0)
        {
            angleX = 0;
            return;
        }
        if (angleX > 90)
        {
            angleX = 90;
            return;
        }
    }

    Coroutine moveCoroutine;
    int playerLayer = 9;
    int enemyLayer = 7;

    public void OnAttackMove(AttackRangeData data)
    {
        if (data == null) return;

        // 기존 이동이 있다면 중지
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        
        moveCoroutine = StartCoroutine(ProcessAttackMove(data));
    }

    private IEnumerator ProcessAttackMove(AttackRangeData data)
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 direction = movement.renderTransform.forward;
        
        // 1. 관통 예외 처리 (Pass Through)
        if (data.passThrough)
        {
            Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        }

        // 2. 이동 실행 (Action_Time 동안 진행)
        // 기획서대로 순간이동이 아닌 Velocity 기반의 부드러운 이동
        float speed = data.moveDist / data.actionTime;

        while (elapsed < data.actionTime)
        {
            // 물리 엔진에 의해 막히는 것은 Rigidbody가 알아서 처리함
            Vector3 moveAmount = direction * speed * Time.deltaTime;
            movement.controller.Move(moveAmount);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 3. 이동 종료 및 초기화
        StopMovement();

        // 4. 관통 상태 원상 복구
        if (data.passThrough)
        {
            Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);
            
            // [추가] 끼임 방지: 적과 겹쳐있다면 살짝 밀어내기 (선택 사항)
            // transform.position = ... (보정 로직)
        }
    }

    public void StopMovement()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        playerRigidbody.velocity = Vector3.zero; // 관성 제거
    }
}
