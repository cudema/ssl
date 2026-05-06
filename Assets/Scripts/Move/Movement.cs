using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [ReadOnly]
    [SerializeField]
    float speed;
    [ReadOnly]
    [SerializeField]
    float jumpSpeed;
    [ReadOnly]
    [SerializeField]
    float rotationSpeed;
    public Transform renderTransform;

    [HideInInspector]
    CharacterController controller;

    public CharacterController Controller
    {
        get => controller;
    }

    float gravity = 0;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        renderTransform = transform.GetChild(0).transform;
    }
    public void OnGravity()
    {
        if (controller.isGrounded)
        {
            gravity = -1f;
        }
        controller.Move(new Vector3(0, gravity * Time.deltaTime, 0));
        gravity += -9.8f * Time.deltaTime;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetSpeed(float newSpeed, float rotationSpeed)
    {
        speed = newSpeed;
        this.rotationSpeed = rotationSpeed;
    }

    public void ToMove(Vector3 direction)
    {
        controller.Move(direction * Time.fixedDeltaTime * speed);
        if (direction != Vector3.zero)
        {
            LookAt(direction);
        }
    }

    public void ToMove(Vector3 direction, float speed)
    {
        controller.Move(direction * Time.fixedDeltaTime * speed);
        if (direction != Vector3.zero)
        {
            LookAt(Player.instance.transform.position - transform.position);
        }
    }

    public Transform GetTransform()
    {
        return renderTransform;
    }

    void LookAt(Vector3 direction)
    {
        Quaternion tempDir = Quaternion.LookRotation(direction);
        renderTransform.rotation = Quaternion.Lerp(renderTransform.rotation, tempDir, Time.deltaTime * rotationSpeed);
    }

    public void FastLookAt(Vector3 direction)
    {
        Quaternion tempDir = Quaternion.LookRotation(direction);
        renderTransform.rotation = tempDir;
    }

    public void LookAtTarget(Vector3 targetVector)
    {
        //Debug.Log("LookTarget");
        if (targetVector == Vector3.zero)
        {
            return;
        }

        Vector3 dir = targetVector - transform.position;
        dir.y = 0;

        Quaternion tempDir = Quaternion.LookRotation(dir);

        StartCoroutine(Look(tempDir));
    }

    IEnumerator Look(Quaternion dir)
    {
        float tempTime = Time.time;

        while(Time.time - tempTime < 0.1f)
        {
            renderTransform.rotation = Quaternion.Lerp(renderTransform.rotation, dir, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
