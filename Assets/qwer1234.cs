using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qwer1234 : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    Animator cameraanimator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetTrigger("BearSlash");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetTrigger("GroundSmash");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetTrigger("GroundBomb");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            cameraanimator.SetTrigger("1");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            cameraanimator.SetTrigger("2");
        }
    }
}
