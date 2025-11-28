using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField]
    StageData currentData;

    [SerializeField]
    Material[] materials;

    public void Setup(StageType type)
    {
        string path = "StageData/" + type.ToString();
        StageData[] tempDatas = Resources.LoadAll<StageData>(path);
        currentData = tempDatas[UnityEngine.Random.Range(0, tempDatas.Length)];
        transform.GetChild(0).GetComponent<Renderer>().material = materials[(int)type];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //다른 스테이지로 이동
            StageManager.instance.SetStage(currentData);
        }
    }
}
