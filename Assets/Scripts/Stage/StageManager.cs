using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

public enum StageType
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
    [SerializeField]
    GameObject currentStage;

    [SerializeField]
    Image fadePanel;
    [SerializeField]
    float fadeTime;

    Coroutine stageSpowning;

    [SerializeField]
    GameObject portal;

    Transform[] portalSpownPoints;

    bool isPlayStage;

    List<MemoryPool> enemyPool = new List<MemoryPool>();

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

    public void AddCountDeadEnemy(GameObject deadEnemy)
    {
        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (enemyPool[i].OnDeactiveObjec(deadEnemy))
            {
                break;
            }
        }

        currnetDeadCount++;
    }

    void Update()
    {
        if (clearDeadCount == currnetDeadCount && isPlayStage)
        {
            isPlayStage = false;
            //Debug.Log("스테이지 클리어");
            foreach (MemoryPool pool in enemyPool)
            {
                pool.DestroyPool();
            }
            enemyPool.Clear();

            for (int i = 0; i < portalSpownPoints.Length; i++)
            {
                Portal tempPortal = Instantiate(portal, portalSpownPoints[i]).GetComponent<Portal>();
                tempPortal.Setup(StageType.Combat);
            }
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

                GameObject tempEnemy = enemyPool[data.EnmeyGroup[currentIndex].enemyIndex].OnActiveObject(new Vector3(transform.position.x + tempPositionX, transform.position.y + 1, transform.position.z + tempPositionZ));
                tempEnemy.GetComponent<EnemyBase>().Setup(this);
            }
        }
    }

    public void SetStage(StageData stageData)
    {
        data = stageData;

        StartCoroutine(StageSetting());
    }

    IEnumerator StageSetting()
    {
        Player.instance.StopPlayer();

        while (fadePanel.color.a <= 1)
        {
            fadePanel.color += new Color(0, 0, 0, 1 / fadeTime * Time.deltaTime);

            yield return null;
        }

        Destroy(currentStage);

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

        yield return null;

        currentStage = Instantiate(data.StageFild);
        
        yield return null;

        var spownTemp = currentStage.transform.GetChild(0).GetComponentsInChildren<Transform>();
        spownPoint = spownTemp.Where(c => c.gameObject != currentStage.transform.GetChild(0).gameObject).ToArray();

        yield return null;

        var portalTemp = currentStage.transform.GetChild(2).GetComponentsInChildren<Transform>();
        portalSpownPoints = portalTemp.Where(c => c.gameObject != currentStage.transform.GetChild(2).gameObject).ToArray();

        yield return null;

        Player.instance.OnPositionSet(currentStage.transform.GetChild(1).transform.position);

        yield return null;

        for (int i = 0; i < data.EnemyPrefab.Length; i++)
        {
            enemyPool.Add(new MemoryPool(data.EnemyPrefab[i]));
        }

        yield return null;

        while (fadePanel.color.a >= 0)
        {
            fadePanel.color -= new Color(0, 0, 0, 1 / fadeTime * Time.deltaTime);

            yield return null;
        }

        Player.instance.SetupPlayer();

        yield return new WaitForSeconds(1f);

        isPlayStage = true;
        stageSpowning = StartCoroutine(BingStage());
    }

    public void EndRun()
    {
        Player.instance.OnPlayerStatReset();
        StopCoroutine(stageSpowning);
        SceneManager.LoadScene("StartMenu");
    }

    void OpenRewardSelecter()
    {
        
    }
}
