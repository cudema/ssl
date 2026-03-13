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
    public EnemyIndex enemyIndex;
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

public enum EnemyIndex
{
    Enemy0 = 0,
    EndOfFerocious,
    Enemy1
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [SerializeField]
    GameObject[] enemyPrefab;

    [SerializeField]
    StageNode node;
    Transform[] spownPoint;
    //[SerializeField]
    //GameObject currentStage;

    [SerializeField]
    Image fadePanel;
    [SerializeField]
    float fadeTime;

    Coroutine stageSpowning;
    Coroutine stageStart;

    [SerializeField]
    GameObject portal;
    [SerializeField]
    CoinParticleSystem coinParticleSystem;
    Transform[] portalSpownPoints;

    bool isPlayStage;

    List<MemoryPool> enemyPool = new List<MemoryPool>();

    int clearDeadCount = 0;
    int currnetDeadCount = 0;
    int currentTurn = 0;
    int maxStageTurn = 2;

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
        //Debug.Log(currnetDeadCount);
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

            StartCoroutine(ClearStage());
        }
    }

    IEnumerator BingStage()
    {
        while (randomDataList.Count > 0)
        {
            SpownEnemy();

            yield return new WaitForSeconds(node.Data.WaveDilayTime);
        }
    }

    void SpownEnemy()
    {
        foreach (Transform transform in spownPoint)
        {
            //Debug.Log(randomDataList.Count);
            if (randomDataList.Count <= 0)
            {
                return;
            }
            int temp = Random.Range(0, randomDataList.Count);
            int currentIndex = randomDataList[temp];
            randomDataList.RemoveAt(temp);

            for (int i = 0; i < node.Data.EnmeyGroup[currentIndex].enemyCount; i++)
            {
                float tempPositionX = Random.Range(-3f, 3f);
                float tempPositionZ = Random.Range(-3f, 3f);

                GameObject tempEnemy = enemyPool[(int)node.Data.EnmeyGroup[currentIndex].enemyIndex].OnActiveObject(new Vector3(transform.position.x + tempPositionX, transform.position.y + 1, transform.position.z + tempPositionZ));
                tempEnemy.GetComponent<EnemyBase>().Setup(this);
            }
        }
    }

    public void SetStage(StageNode stageNode)
    {
        if (stageStart != null)
        {
            StopCoroutine(stageStart);
        }

        node = stageNode;

        if (!node.IsVisited)
        {
            stageStart = StartCoroutine(StageSetting());
            currentTurn++;
            Debug.Log("StatStage");
        }
    }

    IEnumerator StageSetting()
    {
        //Player.instance.StopPlayer();

        // while (fadePanel.color.a <= 1)
        // {
        //     fadePanel.color += new Color(0, 0, 0, 1 / fadeTime * Time.deltaTime);

        //     yield return null;
        // }

        //Destroy(currentStage);

        clearDeadCount = 0;
        currnetDeadCount = 0;

        randomDataList.Clear();
        node.CloseDoor();

        for(int i = 0; i < node.Data.EnmeyGroup.Length; i++)
        {
            clearDeadCount += node.Data.EnmeyGroup[i].enemyCount * node.Data.EnmeyGroup[i].useCount;
            for (int j =0; j < node.Data.EnmeyGroup[i].useCount; j++)
            {
                randomDataList.Add(i);
            }
        }

        yield return null;

        //currentStage = Instantiate(data.StageFild);
        //Debug.Log(currentStage);
        
        yield return null;

        spownPoint = node.SpownPoints;

        // yield return null;

        // var portalTemp = currentStage.transform.GetChild(2).GetComponentsInChildren<Transform>();
        // portalSpownPoints = portalTemp.Where(c => c.gameObject != currentStage.transform.GetChild(2).gameObject).ToArray();

        //yield return null;
        //Debug.Log(currentStage.transform.GetChild(1).transform.position);
        //Player.instance.OnPositionSet(currentStage.transform.GetChild(1).transform.position);

        yield return null;

        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            enemyPool.Add(new MemoryPool(enemyPrefab[i]));
        }

        yield return null;

        // while (fadePanel.color.a >= 0)
        // {
        //     fadePanel.color -= new Color(0, 0, 0, 1 / fadeTime * Time.deltaTime);

        //     yield return null;
        // }

        //Player.instance.SetupPlayer();

        yield return new WaitForSeconds(1f);

        isPlayStage = true;
        stageSpowning = StartCoroutine(BingStage());
    }

    public void EndRun()
    {
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        Player.instance.OnPlayerStatReset();
        
        if (stageSpowning != null)
        {
            StopCoroutine(stageSpowning);
        }

        yield return new WaitForSeconds(3f);

        foreach (MemoryPool pool in enemyPool)
        {
            pool.DestroyPool();
        }
        enemyPool.Clear();

        SceneManager.LoadScene("GameOver");
        Player.instance.OnPlayer();
        Player.instance.OnPositionSet(new Vector3(0, 0, 0));
    }

    IEnumerator ClearStage()
    {
        int coin = node.Data.dropCoin;
        coinParticleSystem.transform.position = node.transform.position;
        coinParticleSystem.OnCoinParticlePlay(CoinType.Coin_S, coin % 10);
        coinParticleSystem.OnCoinParticlePlay(CoinType.Coin_L, coin / 10);

        yield return new WaitForSeconds(2f);

        Player.instance.StopPlayer();

        // UIManager.instance.statAdder.SetStat();

        // yield return new WaitWhile(() => UIManager.instance.statAdder.isSelectingStat);

        // for (int i = 0; i < portalSpownPoints.Length; i++)
        // {
        //     Portal tempPortal = Instantiate(portal, portalSpownPoints[i]).GetComponent<Portal>();
        //     tempPortal.Setup(StageType.Combat);
        // }

        Player.instance.SetupPlayer();

        if (currentTurn >= maxStageTurn)
        {
            yield break;
        }

        node.OpenDoor();
    }
}
