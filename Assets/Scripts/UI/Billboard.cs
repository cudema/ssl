using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        // 메인 카메라의 트랜스폼을 참조합니다.
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        // UI가 항상 카메라를 바라보도록 회전값을 고정합니다.
        transform.LookAt(transform.position + cam.forward);
    }
}
