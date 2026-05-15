using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace HexPosition
{
    public static class HexPos
    {
        public static Vector2Int WorldToHex(Vector3 worldPos)
        {
            float q = (2f / 3f * worldPos.x) / 24.125f;
            float r = (-1f / 3f * worldPos.x + Mathf.Sqrt(3f) / 3f * worldPos.z) / 24.125f;
            return HexRound(q, r);
        }

        public static Vector2Int HexRound(float q, float r)
        {
            float s = -q - r;

            int qi = Mathf.RoundToInt(q);
            int ri = Mathf.RoundToInt(r);
            int si = Mathf.RoundToInt(s);

            float q_diff = Mathf.Abs(qi - q);
            float r_diff = Mathf.Abs(ri - r);
            float s_diff = Mathf.Abs(si - s);

            if (q_diff > r_diff && q_diff > s_diff)
            {
                qi = -ri - si;
            }
            else if (r_diff > s_diff)
            {
                ri = -qi - si;
            }

            return new Vector2Int(qi, ri);
        }

        public static List<T> GetObjectsInRange<T>(Dictionary<Vector2Int, T> objs, Vector2Int centerAxial, int range) where T : class
        {
            List<T> results = new List<T>();

            // 중심 좌표를 큐브 좌표로 변환
            int centerX = centerAxial.x;
            int centerZ = centerAxial.y;
            int centerY = -centerX - centerZ;

            // 범위 내의 모든 큐브 좌표 순회
            for (int dx = -range; dx <= range; dx++)
            {
                for (int dy = Mathf.Max(-range, -dx - range); dy <= Mathf.Min(range, -dx + range); dy++)
                {
                    int dz = -dx - dy;

                    // 다시 축 좌표(q, r)로 변환하여 Dictionary에서 검색
                    Vector2Int targetPos = new Vector2Int(centerX + dx, centerZ + dz);

                    if (objs.TryGetValue(targetPos, out T obj))
                    {
                        results.Add(obj);
                    }
                }
            }
            return results;
        }

        public static List<T> GetObjectsAtExactDistance<T>(Dictionary<Vector2Int, T> objs, Vector2Int centerAxial, int distance) where T : class
        {
            List<T> results = new List<T>();
            if (distance == 0)
            {
                if (objs.TryGetValue(centerAxial, out T obj)) results.Add(obj);
                return results;
            }

            // 육각형의 6방향 벡터 (큐브 좌표 기준)
            Vector3Int[] directions = {
                new Vector3Int(1, -1, 0), new Vector3Int(1, 0, -1), new Vector3Int(0, 1, -1),
                new Vector3Int(-1, 1, 0), new Vector3Int(-1, 0, 1), new Vector3Int(0, -1, 1)
            };

            // 1. 시작 지점 설정: 중심에서 한 방향으로 distance만큼 떨어진 곳
            Vector3Int currentCube = AxialToCube(centerAxial) + (directions[4] * distance);

            // 2. 6개의 방향을 돌며 각각 distance번만큼 이동
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < distance; j++)
                {
                    Vector2Int currentAxial = CubeToAxial(currentCube);
                    
                    if (objs.TryGetValue(currentAxial, out T obj))
                    {
                        results.Add(obj);
                    }
                    // 다음 인접 타일로 이동
                    currentCube += directions[i];
                }
            }
            return results;
        }

        public static List<T> SortObjectsList<T>(List<T> values, Vector2Int center) where T : StageNode
        {
            return values.OrderByDescending(values => GetHexDistance(center, values.pos)).ToList();
        }

        public static int GetHexDistance(Vector2Int a, Vector2Int b)
        {
            int dq = a.x - b.x;
            int dr = a.y - b.y;
            return (Mathf.Abs(dq) + Mathf.Abs(dr) + Mathf.Abs(-dq - dr)) / 2;
        }

        public static bool ChackOjbectOfDistans(Dictionary<Vector2Int, StageNode> objs, Vector2Int centerAxial, int distance, StageType chackType)
        {
            var temps = GetObjectsInRange(objs, centerAxial, distance).Where(a => a.type == chackType).ToList();

            if (temps.Count == 0)
            {
                return true;
            }

            return false;
        }

        private static Vector3Int AxialToCube(Vector2Int axial)
        {
            return new Vector3Int(axial.x, -axial.x - axial.y, axial.y);
        }

        private static Vector2Int CubeToAxial(Vector3Int cube)
        {
            return new Vector2Int(cube.x, cube.z);
        }
    }
}