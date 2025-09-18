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
    Transform renderTransform;

    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        renderTransform = GetComponentInChildren<Renderer>().transform;
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
}
