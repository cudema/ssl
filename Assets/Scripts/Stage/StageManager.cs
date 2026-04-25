using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    //Smithy,
    //Event,
    Rest,
    Boss
}

public enum EnemyIndex
{
    Enemy0 = 0,
    EndOfFerocious,
    UnleashedDemon,
    EB_Melee_01,
    EB_Melee_02,
    EB_Melee_03,
    EB_Range_01,
    EB_Range_02
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [SerializeField]
    GameObject[] enemyPrefab;

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

    Camera minimapCamera;

    GameObject portal;
    [SerializeField]
    CoinParticleSystem coinParticleSystem;
    Transform[] portalSpownPoints;

    [SerializeField]
    Material currentMapColor;
    [SerializeField]
    Material claerMapColor;

    bool isPlayStage;

    TextMeshProUGUI trunCountText;

    List<MemoryPool> enemyPool = new List<MemoryPool>();

    int clearDeadCount = 0;
    int currnetDeadCount = 0;
    int currentTurn = -1;
    int maxStageTurn = 6;

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
        DontDestroyOnLoad(this);
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
            //Debug.Log("스테이지 클리어");
            isPlayStage = false;

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
                float tempPositionX = Random.Range(-2f, 2f);
                float tempPositionZ = Random.Range(-2f, 2f);

                GameObject tempEnemy = enemyPool[(int)node.Data.EnmeyGroup[currentIndex].enemyIndex].OnActiveObject(new Vector3(transform.position.x + tempPositionX, transform.position.y + 1, transform.position.z + tempPositionZ));
                tempEnemy.GetComponent<EnemyBase>().Setup(this);
            }
        }
    }

    public void SetStage(StageNode stageNode)
    {
        node = stageNode;

        // if (stageStart != null)
        // {
        //     StopCoroutine(stageStart);
        // }

        if (!node.IsVisited)
        {
            currentTurn++;
            trunCountText.text = (maxStageTurn - currentTurn).ToString();
            Debug.Log("StatStage");
            node.MapRanderer.material = currentMapColor;

            switch (node.Data.stageType)
            {
                case StageType.Combat:
                    stageStart = StartCoroutine(StageSetting());
                    break;
                case StageType.Boss:
                    stageStart = StartCoroutine(StageSetting());
                    break;
                case StageType.Elite:
                    stageStart = StartCoroutine(StageSetting());
                    break;
                case StageType.Rest:
                    stageStart = StartCoroutine(RestStageSetting());
                    break;
                // case StageType.Event:
                //     break;
                case StageType.Shop:
                    stageStart = StartCoroutine(ShopStageSetting());
                    break;
                // case StageType.Smithy:
                //     break;
                case StageType.Treasure:
                    stageStart = StartCoroutine(TreasureStageSetting());
                    break;
            }
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

        spownPoint = node.SpownPoints;

        // yield return null;

        // var portalTemp = currentStage.transform.GetChild(2).GetComponentsInChildren<Transform>();
        // portalSpownPoints = portalTemp.Where(c => c.gameObject != currentStage.transform.GetChild(2).gameObject).ToArray();

        //yield return null;
        //Debug.Log(currentStage.transform.GetChild(1).transform.position);
        //Player.instance.OnPositionSet(currentStage.transform.GetChild(1).transform.position);

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

    IEnumerator RestStageSetting()
    {
        clearDeadCount = 0;
        currnetDeadCount = 0;

        randomDataList.Clear();
        node.CloseDoor();

        yield return null;

        //currentStage = Instantiate(data.StageFild);
        //Debug.Log(currentStage);

        Campfire campfire = Instantiate(node.Data.restStageData.obj, node.transform.position + new Vector3(0, 1, 0), Quaternion.identity).GetComponent<Campfire>();
        campfire.SetValue(node.Data.restStageData.value);

        yield return new WaitForSeconds(1f);

        StartCoroutine(ClearStage());
    }

    IEnumerator ShopStageSetting()
    {
        clearDeadCount = 0;
        currnetDeadCount = 0;

        randomDataList.Clear();
        node.CloseDoor();

        yield return null;

        //currentStage = Instantiate(data.StageFild);
        //Debug.Log(currentStage);

        Instantiate(node.Data.shopStageData.obj, node.transform.position + new Vector3(0, 1, 0), Quaternion.identity).GetComponent<Campfire>();

        yield return new WaitForSeconds(1f);

        StartCoroutine(ClearStage());
    }

    IEnumerator TreasureStageSetting()
    {
        clearDeadCount = 0;
        currnetDeadCount = 0;

        randomDataList.Clear();
        node.CloseDoor();

        yield return null;

        //currentStage = Instantiate(data.StageFild);
        //Debug.Log(currentStage);

        Instantiate(node.Data.treasureStageData.obj, node.transform.position + new Vector3(0, 1, 0), Quaternion.identity).GetComponent<Campfire>();

        yield return new WaitForSeconds(1f);

        StartCoroutine(ClearStage());
    }

    public void StartScene()
    {
        StageStartData temp = FindObjectOfType<StageStartData>();
        maxStageTurn = temp.trunCount;
        portal = temp.bossPortal;
        trunCountText = temp.trunCountText;
        trunCountText.text = maxStageTurn.ToString();
        currentTurn = -1;
        minimapCamera = GameObject.FindGameObjectWithTag("MiniMap")?.GetComponent<Camera>();
        Player.instance.OnPositionSet(temp.transform.position, temp.transform.rotation);
        Player.instance.SetupPlayer();
        
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            enemyPool.Add(new MemoryPool(enemyPrefab[i]));
        }
    }

    public void MoveMiniMap()
    {
        if (minimapCamera == null) return;
        minimapCamera.transform.position = node.transform.position;
    }

    public void RotateCamera(float yValue)
    {
        if (minimapCamera == null) return;
        minimapCamera.transform.rotation = Quaternion.Euler(new Vector3(90, yValue, 0));
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

        SceneControlManager.instance.LoadScene(SceneName.GameOver);
        Player.instance.OnPlayer();
        Player.instance.OnPositionSet(new Vector3(0, 0, 0), Quaternion.identity);
    }

    IEnumerator ClearStage()
    {
        int coin = node.Data.dropCoin;
        coinParticleSystem.transform.position = node.transform.position;
        coinParticleSystem.OnCoinParticlePlay(CoinType.Coin_S, coin % 10);
        coinParticleSystem.OnCoinParticlePlay(CoinType.Coin_L, coin / 10);

        yield return new WaitForSeconds(2f);

        //Player.instance.StopPlayer();

        // UIManager.instance.statAdder.SetStat();

        // yield return new WaitWhile(() => UIManager.instance.statAdder.isSelectingStat);

        // for (int i = 0; i < portalSpownPoints.Length; i++)
        // {
        //     Portal tempPortal = Instantiate(portal, portalSpownPoints[i]).GetComponent<Portal>();
        //     tempPortal.Setup(StageType.Combat);
        // }

        //Player.instance.SetupPlayer();

        node.MapRanderer.material = claerMapColor;

        if (currentTurn >= maxStageTurn)
        {
            Instantiate(portal, node.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            
            foreach (MemoryPool pool in enemyPool)
            {
                pool.DestroyPool();
            }
            enemyPool.Clear();

            yield break;
        }

        node.OpenDoor();
    }
}
