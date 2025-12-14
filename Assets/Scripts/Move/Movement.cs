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
    float rotationSpeed = 10;
    public Transform renderTransform;

    CharacterController controller;

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
            gravity = 0;
        }
        controller.Move(new Vector3(0, gravity * Time.deltaTime, 0));
        gravity += -9.8f * Time.deltaTime;
    }

    public void SetSpeed(float newSpeed, float rotationSpeed)
    {
        speed = newSpeed;
        this.rotationSpeed = rotationSpeed;
    }

    public void ToMove(Vector3 direction)
    {
        controller.Move(direction * Time.deltaTime * speed);
        if (direction != Vector3.zero)
        {
            LookAt(direction);
        }
    }

    public void ToJump()
    {

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
}
