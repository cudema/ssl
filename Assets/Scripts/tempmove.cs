using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempmove : MonoBehaviour
{
    [SerializeField]
    float speed;
    Movement movement;

    void Awake()
    {
        movement = GetComponent<Movement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        movement.SetSpeed(speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            movement.ToMove(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement.ToMove(Vector3.back);
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement.ToMove(Vector3.left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement.ToMove(Vector3.right);
        }
    }
}
