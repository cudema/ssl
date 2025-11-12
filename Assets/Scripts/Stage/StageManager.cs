using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[System.Serializable]
public class StageData
{
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    int enemyCount;
    [SerializeField]
    int useCount = 1;

    public GameObject EnemyPrefab
    {
        get => enemyPrefab;
    }

    public int EnemyCount
    {
        get => enemyCount;
    }

    public int UseCount
    {
        get => useCount;
    }
}

public class StageManager : MonoBehaviour
{
    [SerializeField]
    StageData[] datas;
    [SerializeField]
    Transform[] spownPoint;
    [SerializeField]
    float waveDilayTime;

    int clearDeadCount = 0;
    int currnetDeadCount = 0;

    List<int> randomDataList = new List<int>();

    void Awake()
    {
        for(int i = 0; i < datas.Length; i++)
        {
            clearDeadCount += datas[i].EnemyCount * datas[i].UseCount;
            for (int j =0; j < datas[i].UseCount; j++)
            {
                randomDataList.Add(i);
            }
        }
    }

    void Start()
    {
        StartCoroutine(BingStage());
    }

    public void AddCountDeadEnemy()
    {
        currnetDeadCount++;
    }

    void Update()
    {
        if (clearDeadCount == currnetDeadCount)
        {
            Debug.Log("스테이지 클리어");
        }
    }

    IEnumerator BingStage()
    {
        while (randomDataList.Count > 0)
        {
            SpownEnemy();

            yield return new WaitForSeconds(waveDilayTime);
        }
    }

    void SpownEnemy()
    {
        foreach (Transform transform in spownPoint)
        {
            if (randomDataList.Count <= 0)
            {
                return;
            }
            int temp = Random.Range(0, randomDataList.Count);
            StageData data = datas[randomDataList[temp]];
            randomDataList.RemoveAt(temp);

            for (int i = 0; i < data.EnemyCount; i++)
            {
                float tempPositionX = Random.Range(-5f, 5f);
                float tempPositionZ = Random.Range(-5f, 5f);

                GameObject tempEnemy = Instantiate(data.EnemyPrefab, new Vector3(transform.position.x + tempPositionX, transform.position.y, transform.position.z + tempPositionZ), Quaternion.identity);
                tempEnemy.GetComponent<EnemyBase>().Setup(this);
            }
        }
    }
}
