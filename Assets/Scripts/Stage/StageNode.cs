using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNode : MonoBehaviour
{
    bool isVisited = false;

    [SerializeField]
    Transform[] spownPoints;

    [SerializeField]
    GameObject[] doors;

    [SerializeField]
    StageData data;

    Renderer mapRanderer;

    public bool IsVisited
    {
        get => isVisited;
    }
    public StageData Data
    {
        get => data;
    }

    public Transform[] SpownPoints
    {
        get => spownPoints;
    }

    public Renderer MapRanderer
    {
        get => mapRanderer;
    }

    void Awake()
    {
        mapRanderer = GetComponentInChildren<Renderer>();
    }

    public void VisitStageNode()
    {
        StageManager.instance.SetStage(this);
        isVisited = true;
        StageManager.instance.MoveMiniMap();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VisitStageNode();
        }
    }

    public void OpenDoor()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }
    }

    public void CloseDoor()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(true);
        }
    }
}