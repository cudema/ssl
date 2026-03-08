using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNode : MonoBehaviour
{
    bool isVisited = false;

    [SerializeField]
    Transform[] spownPoints;

    [SerializeField]
    StageData data;

    public StageData Data
    {
        get => data;
    }

    public Transform[] SpownPoints
    {
        get => spownPoints;
    }

    public void VisitStageNode()
    {
        if (!isVisited)
        {
            isVisited = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VisitStageNode();
        }
    }
}