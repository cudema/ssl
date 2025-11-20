using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField]
    StageData stageData;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //다른 스테이지로 이동
            StageManager.instance.SetStage(stageData);
        }
    }
}
