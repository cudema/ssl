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

    [HideInInspector]
    public bool isSetStageData = false;
    [HideInInspector]
    public bool isTreasureOrRset = false;

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

    public void SetStageData(StageType stageType)
    {
        if (isSetStageData) return;

        string path = $"StageData/{stageType}/{StageManager.instance.CurrentStageStartData.stageName}";

        StageData[] tempData = Resources.LoadAll<StageData>(path);

        data = tempData[Random.Range(0, tempData.Length)];

        switch (stageType)
        {
            case StageType.Combat:
                mapRanderer.material = StageManager.instance.combatMapColor;
                break;
            case StageType.Treasure:
                mapRanderer.material = StageManager.instance.treasureMapColor;
                break;
            case StageType.Rest:
                mapRanderer.material = StageManager.instance.restMapColor;
                break;
            default:
                break;
        }

        isSetStageData = true;
    }

    public void StageDataTrigger(int treasureRange, int rsetRange)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 45f, 1 << LayerMask.NameToLayer("StageNode"));
        List<StageNode> stageNode = new List<StageNode>();

        int treasure = treasureRange;
        int rset = rsetRange;

        foreach (Collider temp in hitColliders)
        {
            StageNode nodeTemp;
            if (temp.TryGetComponent(out nodeTemp))
            {
                if (!nodeTemp.isSetStageData)
                {
                    stageNode.Add(nodeTemp);
                }
            }
        }

        if (stageNode.Count == 0)
        {
            return;
        }

        List<StageType> stageTypes = new List<StageType>();

        foreach (StageNode temp in stageNode)
        {
            if (temp.isTreasureOrRset)
            {
                treasure = 0;
                rset = 0;
                break;
            }
        }

        if (Random.Range(0f, 1f) < StageManager.instance.treasureProbability[treasure++])
        {
            stageTypes.Add(StageType.Treasure);
            treasure = 0;
        }

        if (stageTypes.Count < stageNode.Count && Random.Range(0f, 1f) < StageManager.instance.treasureProbability[rset++])
        {
            stageTypes.Add(StageType.Rest);
            rset = 0;
        }

        for (int i = stageTypes.Count; i < stageNode.Count; i++)
        {
            stageTypes.Add(StageType.Combat);
        }

        StartCoroutine(tempDebug(stageNode, stageTypes, treasure, rset));

        // foreach (StageNode temp in stageNode)
        // {
        //     int randomTemp = Random.Range(0, stageTypes.Count);
        //     temp.SetStageData(stageTypes[randomTemp]);
        //     stageTypes.RemoveAt(randomTemp);
        // }

        // foreach(StageNode temp in stageNode)
        // {
        //     temp.StageDataTrigger(treasure, rset);
        // }
    }

    IEnumerator tempDebug(List<StageNode> stageNode, List<StageType> stageTypes, int a, int b)
    {
        foreach (StageNode temp in stageNode)
        {
            int randomTemp = Random.Range(0, stageTypes.Count);
            temp.SetStageData(stageTypes[randomTemp]);
            if (stageTypes[randomTemp] == StageType.Treasure || stageTypes[randomTemp] == StageType.Rest)
            {
                temp.isTreasureOrRset = true;
                a = 0;
                b = 0;
            }
            stageTypes.RemoveAt(randomTemp);
            yield return new WaitForSeconds(0.1f);
        }

        foreach(StageNode temp in stageNode)
        {
            temp.StageDataTrigger(a, b);
            yield return new WaitForSeconds(0.2f);
        }
    }
}