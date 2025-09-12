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

    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ToMove(Vector3 direction)
    {
        controller.Move(direction * Time.deltaTime * speed);
        LookAt(direction);
    }

    public void ToJump()
    {

    }

    void LookAt(Vector3 direction)
    {
        transform.LookAt(transform.position + direction);
    }
}
