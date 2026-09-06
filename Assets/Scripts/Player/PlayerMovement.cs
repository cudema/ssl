using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
public class PlayerMovement : MonoBehaviour
{
    const float MouseDeltaReferenceFrameRate = 120f;

    [SerializeField]
    public float rotationSpeed = 10;
    [SerializeField, Min(1f)]
    float movementSpeedMultiplier = 1.5f;
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

    [HideInInspector]
    public float addAngleSpeed = 1f;

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

    SearchEnemy searchEnemy;

    [Header("Attack Movement")]
    [SerializeField]
    AnimationCurve normalAttackMoveCurve = new AnimationCurve(
        new Keyframe(0f, 0f, 0f, 0f),
        new Keyframe(0.1f, 0.05f, 0.8f, 0.8f),
        new Keyframe(0.42f, 0.82f, 1.25f, 1.25f),
        new Keyframe(0.75f, 0.97f, 0.25f, 0.25f),
        new Keyframe(1f, 1f, 0f, 0f));
    [SerializeField]
    AnimationCurve evadeFollowUpMoveCurve = new AnimationCurve(
        new Keyframe(0f, 0f, 2.4f, 2.4f),
        new Keyframe(0.25f, 0.55f, 1.3f, 1.3f),
        new Keyframe(0.7f, 0.92f, 0.4f, 0.4f),
        new Keyframe(1f, 1f, 0f, 0f));
    [SerializeField, Range(0.5f, 1f)]
    float normalAttackMoveDurationScale = 0.8f;
    [SerializeField, Min(1f)]
    float normalAttackMoveDistanceScale = 1.2f;
    [SerializeField, Range(0.03f, 0.2f)]
    float evadeFollowUpMoveDuration = 0.08f;
    [SerializeField, Min(0f)]
    float evadeFollowUpWindow = 0.25f;
    [SerializeField, Min(0f)]
    float evadeFollowUpRange = 7f;
    [SerializeField, Min(0f)]
    float evadeFollowUpSurfaceGap = 0.05f;

    float evadeFollowUpExpiresAt = float.NegativeInfinity;
    bool useNormalAttackMoveCurve;
    bool applyEvadeFollowUpToNextAttackMove;
    float normalAttackPlaybackDurationScale = 1f;

    void Awake()
    {
        movement = GetComponent<Movement>();
        Vector3 cameraEulerAngles = cameraAngle.localEulerAngles;
        angleX = NormalizeAngle(cameraEulerAngles.x);
        angleY = cameraEulerAngles.y;
        angleMoveCorutine = ToAngle();
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        searchEnemy = GetComponent<SearchEnemy>();
    }

    void Start()
    {
        //SpeedSet();
        addAngleSpeed = PlayerPrefs.GetFloat("MaousRec");
        if (addAngleSpeed == 0)
        {
            addAngleSpeed = 1;
            PlayerPrefs.SetFloat("MaousRec", addAngleSpeed);
        }
    }

    public void SpeedSet()
    {
        SetMovementSpeed(Player.instance.playerStats.stats[StatType.Speed].Value);
    }

    public void SetMovementSpeed(float baseSpeed)
    {
        movement.SetSpeed(baseSpeed * movementSpeedMultiplier, rotationSpeed);
    }

    void Update()
    {
        dir = Quaternion.AngleAxis(cameraAngle.localEulerAngles.y, transform.up) * playerMoveDirection;
        if (PlayerMoveable && dir != Vector3.zero)
        {
            movement.ToPlayerMove(dir, Time.deltaTime);
        }
        else
        {
            movement.ToPlayerMove(Vector3.zero, Time.deltaTime);
        }

        animator.SetFloat("Speed", Vector3.Distance(Vector3.zero, movement.Controller.velocity));

        if (InputManager.instance.GetInputUseable() && movement.Controller.enabled)
        {
            movement.OnGravity();
        }

    }
    Vector2 tempVector;
    void LateUpdate()
    {
        if (!InputManager.instance.GetInputUseable()) return;
        // Mouse delta is already the distance travelled during this frame.
        // Dividing by the former 120 FPS target preserves the existing sensitivity
        // while removing frame-rate-dependent camera movement.
        float sensitivity = angleSpeed * addAngleSpeed / MouseDeltaReferenceFrameRate;
        angleX -= tempVector.y * sensitivity;
        angleY += tempVector.x * sensitivity;

        ChackAngleX();

        cameraAngle.rotation = Quaternion.Euler(new Vector3(angleX, angleY, 0));
        StageManager.instance.RotateCamera(angleY);

        tempVector = Vector2.zero;
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

    public void ResetInputState()
    {
        playerMoviing = false;
        playerMoveDirection = Vector3.zero;
        tempVector = Vector2.zero;
        evadeFollowUpExpiresAt = float.NegativeInfinity;
        EndNormalAttackMove();

        if (animator != null)
        {
            animator.SetBool("IsMove", false);
        }
    }

    public void ToMoveCameraAngle(InputAction.CallbackContext value)
    {
        //StopCoroutine(angleMoveCorutine);

        tempVector = value.ReadValue<Vector2>();
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
        angleX = movement.renderTransform.eulerAngles.x;
        angleY = movement.renderTransform.eulerAngles.y;
        cameraAngle.rotation = Quaternion.Euler(new Vector3(angleX, angleY, 0));
    }

    public void ResetCameraSet()
    {
        angleX = 0;
        angleY = 0;
        cameraAngle.rotation = Quaternion.Euler(new Vector3(angleX, angleY, 0));
    }

    void ChackAngleX()
    {
        if (angleX < -30)
        {
            angleX = -30;
            return;
        }
        if (angleX > 90)
        {
            angleX = 90;
            return;
        }
    }

    static float NormalizeAngle(float angle)
    {
        return angle > 180f ? angle - 360f : angle;
    }

    Coroutine moveCoroutine;
    int attackMoveStartedFrame = -1;
    int playerLayer = 9;
    int enemyLayer = 7;

    public void PrepareNormalAttackMove(float animationSpeed)
    {
        useNormalAttackMoveCurve = true;
        applyEvadeFollowUpToNextAttackMove = Time.time <= evadeFollowUpExpiresAt;
        normalAttackPlaybackDurationScale = 1f / Mathf.Max(animationSpeed, Mathf.Epsilon);
        evadeFollowUpExpiresAt = float.NegativeInfinity;
    }

    public void EndNormalAttackMove()
    {
        useNormalAttackMoveCurve = false;
        applyEvadeFollowUpToNextAttackMove = false;
        normalAttackPlaybackDurationScale = 1f;
    }

    public void NotifyEvadeEnded()
    {
        evadeFollowUpExpiresAt = Time.time + evadeFollowUpWindow;
    }

    public void OnAttackMove(AttackRangeData data)
    {
        if (data == null) return;
        if (!movement.Controller.enabled) return;

        bool isEvadeFollowUp = useNormalAttackMoveCurve && applyEvadeFollowUpToNextAttackMove;
        applyEvadeFollowUpToNextAttackMove = false;

        AnimationCurve moveCurve = null;
        if (useNormalAttackMoveCurve)
        {
            moveCurve = isEvadeFollowUp ? evadeFollowUpMoveCurve : normalAttackMoveCurve;
        }

        float moveDistance = data.moveDist;
        float durationScale = 1f;
        Vector3 direction = movement.renderTransform.forward;

        if (useNormalAttackMoveCurve)
        {
            if (moveDistance > 0f)
            {
                moveDistance *= normalAttackMoveDistanceScale;
            }

            durationScale = isEvadeFollowUp
                ? evadeFollowUpMoveDuration * normalAttackPlaybackDurationScale
                    / Mathf.Max(data.actionTime, Mathf.Epsilon)
                : normalAttackMoveDurationScale * normalAttackPlaybackDurationScale;
        }

        if (isEvadeFollowUp && moveDistance > 0f)
        {
            // 회피 연계의 감지 범위와 최대 접근 범위를 하나의 값으로 관리한다.
            Collider targetCollider;
            EnemyBase target = searchEnemy.GetEnemy(evadeFollowUpRange, out targetCollider);

            if (target != null)
            {
                Vector3 targetPoint = targetCollider != null
                    ? targetCollider.ClosestPoint(transform.position)
                    : target.transform.position;
                Vector3 toTarget = targetPoint - transform.position;
                toTarget.y = 0f;

                if (toTarget.sqrMagnitude <= Mathf.Epsilon)
                {
                    toTarget = target.transform.position - transform.position;
                    toTarget.y = 0f;
                }

                if (toTarget.sqrMagnitude > Mathf.Epsilon)
                {
                    direction = toTarget.normalized;
                    movement.FastLookAt(direction);
                    float stopDistance = movement.Controller.radius + evadeFollowUpSurfaceGap;
                    float availableDistance = Mathf.Max(0f, toTarget.magnitude - stopDistance);
                    moveDistance = Mathf.Min(evadeFollowUpRange, availableDistance);
                }
            }
        }

        // 기존 이동이 있다면 중지
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        }
        moveCoroutine = StartCoroutine(ProcessAttackMove(
            data,
            moveCurve,
            moveDistance,
            durationScale,
            direction));
        attackMoveStartedFrame = Time.frameCount;
    }

    private IEnumerator ProcessAttackMove(
        AttackRangeData data,
        AnimationCurve moveCurve,
        float moveDistance,
        float durationScale,
        Vector3 direction)
    {
        float elapsed = 0f;
        float previousProgress = 0f;
        float duration = Mathf.Max(data.actionTime * durationScale, Mathf.Epsilon);
        
        // 1. 관통 예외 처리 (Pass Through)
        if (data.passThrough)
        {
            Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        }

        // 이동 곡선은 누적 이동 비율을 나타낸다. 곡선을 바꿔도 최종 이동 거리는 유지된다.
        while (elapsed < duration)
        {
            if (data.ContinuedPursuit)
            {
                LookAtEnemy();
                direction = movement.renderTransform.forward;
            }

            elapsed = Mathf.Min(elapsed + Time.deltaTime, duration);
            float normalizedTime = elapsed / duration;
            float progress = EvaluateMoveProgress(moveCurve, normalizedTime);
            float frameDistance = (progress - previousProgress) * moveDistance;
            Vector3 moveAmount = direction * frameDistance;
            movement.Controller.Move(moveAmount);

            previousProgress = progress;
            yield return null;
        }

        playerRigidbody.velocity = Vector3.zero;

        if (data.passThrough)
        {
            Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        }

        moveCoroutine = null;
        attackMoveStartedFrame = -1;
    }

    static float EvaluateMoveProgress(AnimationCurve curve, float normalizedTime)
    {
        if (curve == null || curve.length == 0)
        {
            return normalizedTime;
        }

        float start = curve.Evaluate(0f);
        float end = curve.Evaluate(1f);
        float range = end - start;

        if (Mathf.Abs(range) <= Mathf.Epsilon)
        {
            return normalizedTime;
        }

        return Mathf.Clamp01((curve.Evaluate(normalizedTime) - start) / range);
    }

    public void StopMovement()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        attackMoveStartedFrame = -1;
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        playerRigidbody.velocity = Vector3.zero; // 관성 제거
    }

    public void StopMovementForActionLock()
    {
        // 일부 무기는 같은 프레임에 공격 이동 후 이동 잠금 이벤트가 호출된다.
        // 그 경우 새로 시작한 공격 이동만 보호하고, 이전 이동은 정상적으로 중단한다.
        if (moveCoroutine != null && attackMoveStartedFrame == Time.frameCount)
        {
            return;
        }

        StopMovement();
    }

    public void LookAtEnemy()
    {
        movement.LookAtTarget(searchEnemy.GetEnemyPos());
    }
}
