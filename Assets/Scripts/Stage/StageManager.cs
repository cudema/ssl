using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

[System.Serializable]
public struct EnemyGroup
{
    [SerializeField]
    public int enemyIndex;
    [SerializeField]
    public int enemyCount;
    [SerializeField]
    public int useCount;
}

enum StageType
{
    Combat = 0,
    Elite,
    Treasure,
    Shop,
    Smithy,
    Event,
    Rest,
    Boss
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [SerializeField]
    StageData data;
    Transform[] spownPoint;
    GameObject currentStage;

    Coroutine stageSpowning;

    [SerializeField]
    GameObject[] portal;

    int clearDeadCount = 0;
    int currnetDeadCount = 0;

    List<int> randomDataList = new List<int>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        
    }

    public void AddCountDeadEnemy()
    {
        currnetDeadCount++;
    }

    void Update()
    {
        if (clearDeadCount == currnetDeadCount)
        {
            //Debug.Log("스테이지 클리어");
        }
    }

    IEnumerator BingStage()
    {
        while (randomDataList.Count > 0)
        {
            SpownEnemy();

            yield return new WaitForSeconds(data.WaveDilayTime);
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
            int currentIndex = randomDataList[temp];
            randomDataList.RemoveAt(temp);

            for (int i = 0; i < data.EnmeyGroup[currentIndex].enemyCount; i++)
            {
                float tempPositionX = Random.Range(-5f, 5f);
                float tempPositionZ = Random.Range(-5f, 5f);

                GameObject tempEnemy = Instantiate(data.EnemyPrefab[data.EnmeyGroup[currentIndex].enemyIndex], new Vector3(transform.position.x + tempPositionX, transform.position.y, transform.position.z + tempPositionZ), Quaternion.identity);
                tempEnemy.GetComponent<EnemyBase>().Setup(this);
            }
        }
    }

    public void SetStage(StageData stageData)
    {
        Debug.Log(1234);
        Destroy(currentStage);

        data = stageData;

        clearDeadCount = 0;
        currnetDeadCount = 0;

        for(int i = 0; i < data.EnmeyGroup.Length; i++)
        {
            clearDeadCount += data.EnmeyGroup[i].enemyCount * data.EnmeyGroup[i].useCount;
            for (int j =0; j < data.EnmeyGroup[i].useCount; j++)
            {
                randomDataList.Add(i);
            }
        }

        currentStage = Instantiate(data.StageFild);
        var temp = currentStage.transform.GetChild(0).GetComponentsInChildren<Transform>();
        spownPoint = temp.Where(c => c.gameObject != currentStage.transform.GetChild(0).gameObject).ToArray();
        stageSpowning = StartCoroutine(BingStage());
    }
}
