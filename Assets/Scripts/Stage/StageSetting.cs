using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HexPosition;

public class StageSetting : MonoBehaviour
{
    Dictionary<Vector2Int, StageNode> nodes = new Dictionary<Vector2Int, StageNode>();

    public float treasureRange;
    public float restRange;
    public float eliteRange;

    public StageSetting(float treasure, float rest, float elite)
    {
        treasureRange = treasure;
        restRange = rest;
        eliteRange = elite;
    }

    public void ReadNode()
    {
        nodes.Clear();
        StageNode[] stageNodes = FindObjectsOfType<StageNode>();

        foreach (StageNode temp in stageNodes)
        {
            temp.pos = HexPos.WorldToHex(temp.transform.position);
            nodes.Add(temp.pos, temp);
        }
        nodes.Remove(StageManager.instance.CurrentStageStartData.startNode.pos);
    }

    public void ResetNode()
    {
        foreach (StageNode temp in nodes.Values)
        {
            temp.ResetColor();
            temp.isSetStageData = false;
            temp.type = StageType.None;
        }
    }

    public void Setting()
    {
        List<StageNode> temps;
        int rantemp;

        int treasureCount = (int)(nodes.Count * treasureRange);
        int restCount = (int)(nodes.Count * restRange);
        int eliteCount = (int)(nodes.Count * eliteRange);

        temps = HexPos.GetObjectsAtExactDistance<StageNode>(nodes, StageManager.instance.CurrentStageStartData.startNode.pos, 2);
        
        while (true)
        {
            rantemp = Random.Range(0, temps.Count);
            if (HexPos.ChackOjbectOfDistans(nodes, temps[rantemp].pos, 1, StageType.Treasure))
            {
                temps[rantemp].SetStageData(StageType.Treasure);
                treasureCount--;
                break;
            }
            temps.RemoveAt(rantemp);
        }
        temps.Clear();

        temps = HexPos.GetObjectsInRange<StageNode>(nodes, StageManager.instance.CurrentStageStartData.startNode.pos, 4);
        temps.RemoveAll(a => HexPos.GetHexDistance(a.pos, StageManager.instance.CurrentStageStartData.startNode.pos) <= 2);

        while (treasureCount > 0)
        {
            rantemp = Random.Range(0, temps.Count);
            if (HexPos.ChackOjbectOfDistans(nodes, temps[rantemp].pos, 1, StageType.Treasure))
            {
                temps[rantemp].SetStageData(StageType.Treasure);
                treasureCount--;
            }
            temps.RemoveAt(rantemp);
        }
        temps.Clear();

        int y = 2;
        temps = nodes.Values.Where(nodes => nodes.type == StageType.None).ToList();
        //List<StageNode> sortedNode = HexPos.SortObjectsList<StageNode>(temps, StageManager.instance.CurrentStageStartData.startNode.pos);
        //temps.RemoveAll(a => HexPos.GetHexDistance(a.pos, StageManager.instance.CurrentStageStartData.startNode.pos) <= 1);
        
        List<Vector2Int> elitePos = new List<Vector2Int>();

        while (eliteCount > 0)
        {
            if (temps.Count == 0)
            {
                y--;
                temps.Clear();
                temps = nodes.Values.Where(nodes => nodes.type == StageType.None).ToList();
                //temps.RemoveAll(a => HexPos.GetHexDistance(a.pos, StageManager.instance.CurrentStageStartData.startNode.pos) <= 1);
            }
            rantemp = Random.Range(0, temps.Count);

            if (HexPos.ChackOjbectOfDistans(nodes, temps[rantemp].pos, y, StageType.Elite))
            {
                temps[rantemp].SetStageData(StageType.Elite);
                elitePos.Add(temps[rantemp].pos);
                eliteCount--;
            }

            temps.RemoveAt(rantemp);
        }

        temps.Clear();

        foreach (Vector2Int tempvector in elitePos)
        {
            temps.AddRange(HexPos.GetObjectsInRange(nodes, tempvector, 2));
        }

        temps = temps.Distinct().ToList();
        temps.RemoveAll(a => a.isSetStageData);

        while (restCount > 0)
        {
            rantemp = Random.Range(0, temps.Count);

            if (HexPos.ChackOjbectOfDistans(nodes, temps[rantemp].pos, 2, StageType.Rest))
            {
                temps[rantemp].SetStageData(StageType.Rest);
                restCount--;
            }

            temps.RemoveAt(rantemp);
        }

        foreach (StageNode temp in nodes.Values)
        {
            if (!temp.isSetStageData)
            {
                temp.SetStageData(StageType.Combat);
            }
        }
    }
}
