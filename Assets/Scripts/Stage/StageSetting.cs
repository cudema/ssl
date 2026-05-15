using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HexPosition;

public class StageSetting : MonoBehaviour
{
    Dictionary<Vector2Int, StageNode> nodes = new Dictionary<Vector2Int, StageNode>();

    public void ReadNode()
    {
        nodes.Clear();
        StageNode[] stageNodes = FindObjectsOfType<StageNode>();

        foreach (StageNode temp in stageNodes)
        {
            temp.pos = HexPos.WorldToHex(temp.transform.position);
            nodes.Add(temp.pos, temp);
        }
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

        temps = HexPos.GetObjectsAtExactDistance<StageNode>(nodes, new Vector2Int(0, 0), 2);

        while (true)
        {
            rantemp = Random.Range(0, temps.Count);
            if (HexPos.ChackOjbectOfDistans(nodes, temps[rantemp].pos, 2, StageType.Treasure))
            {
                temps[rantemp].SetStageData(StageType.Treasure);
                break;
            }
        }
        temps.Clear();

        temps = HexPos.GetObjectsAtExactDistance<StageNode>(nodes, new Vector2Int(0, 0), 3);

        while (true)
        {
            rantemp = Random.Range(0, temps.Count);
            if (HexPos.ChackOjbectOfDistans(nodes, temps[rantemp].pos, 2, StageType.Treasure))
            {
                temps[rantemp].SetStageData(StageType.Treasure);
                break;
            }
        }
        temps.Clear();

        temps = HexPos.GetObjectsAtExactDistance<StageNode>(nodes, new Vector2Int(0, 0), 4);

        while (true)
        {
            rantemp = Random.Range(0, temps.Count);
            if (HexPos.ChackOjbectOfDistans(nodes, temps[rantemp].pos, 2, StageType.Treasure))
            {
                temps[rantemp].SetStageData(StageType.Treasure);
                break;
            }
        }
        temps.Clear();

        temps = nodes.Values.Where(nodes => nodes.type == StageType.None).ToList();
        List<StageNode> sortedNode = HexPos.SortObjectsList<StageNode>(temps, StageManager.instance.CurrentStageStartData.startNode.pos);
        
        List<Vector2Int> elitePos = new List<Vector2Int>();

        int x = 6;
        int y = 3;
        while (x > 0)
        {
            if (sortedNode.Count / 6 == 1)
            {
                y--;
                temps.Clear();
                sortedNode.Clear();
                temps = nodes.Values.Where(nodes => nodes.type == StageType.None).ToList();
                sortedNode = HexPos.SortObjectsList<StageNode>(temps, StageManager.instance.CurrentStageStartData.startNode.pos);
            }
            rantemp = Random.Range(0, sortedNode.Count / 6);

            if (HexPos.ChackOjbectOfDistans(nodes, sortedNode[rantemp].pos, y, StageType.Elite))
            {
                sortedNode[rantemp].SetStageData(StageType.Elite);
                elitePos.Add(sortedNode[rantemp].pos);
                x--;
            }

            sortedNode.RemoveAt(rantemp);
        }

        temps.Clear();

        foreach (Vector2Int tempvector in elitePos)
        {
            temps.AddRange(HexPos.GetObjectsInRange(nodes, tempvector, 2));
        }

        temps = temps.Distinct().ToList();
        temps = temps.Where(a => !a.isSetStageData).ToList();

        x = 3;
        while (x > 0)
        {
            rantemp = Random.Range(0, temps.Count);

            if (HexPos.ChackOjbectOfDistans(nodes, temps[rantemp].pos, 2, StageType.Rest))
            {
                temps[rantemp].SetStageData(StageType.Rest);
                x--;
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
