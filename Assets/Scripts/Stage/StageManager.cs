using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
    None = 0,
    Combat = 1,
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
    EB_Range_02,
    EE_Melee_01,
    EE_Range_01
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [SerializeField]
    GameObject[] enemyPrefab;

    public StageNode node;
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
    Camera minimapCamera;

    GameObject portal;
    [SerializeField]
    CoinParticleSystem coinParticleSystem;
    Transform[] portalSpownPoints;

    [SerializeField, Header("MiniMapColor")]
    Material currentMapColor;
    [SerializeField]
    Material claerMapColor;

    public Material combatMapColor;

    public Sprite treasureMapColor;

    public Sprite restMapColor;
    public Sprite eliteMapColor;

    [HideInInspector]
    public StageStartData CurrentStageStartData;
    [Header("스테이지 확률"), SerializeField, Range(0, 1)]
    float treasureRange;
    [SerializeField, Range(0, 1)]
    float restRange;
    [SerializeField, Range(0, 1)]
    float eliteRange;

    [SerializeField, Range(0, 1)]
    float[] eliteRewardsRange;

    int clearEliteRoom = 0;

    bool isPlayStage;

    TextMeshProUGUI trunCountText;

    Dictionary<EnemyIndex, MemoryPool> enemyPool = new Dictionary<EnemyIndex, MemoryPool>();

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
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    public void AddCountDeadEnemy(GameObject deadEnemy)
    {
        foreach (MemoryPool temp in enemyPool.Values)
        {
            temp.OnDeactiveObjec(deadEnemy);
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


        //--------------임시 디버깅용 코드-------------------
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            stageSetting.ResetNode();
            stageSetting.Setting();
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

                GameObject tempEnemy = enemyPool[node.Data.EnmeyGroup[currentIndex].enemyIndex].OnActiveObject(new Vector3(transform.position.x + tempPositionX, transform.position.y + 1, transform.position.z + tempPositionZ));
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

        ShopOpener shopOpener = Instantiate(node.Data.shopStageData.obj, node.transform.position + new Vector3(0, 1, 0), Quaternion.identity).GetComponent<ShopOpener>();
        shopOpener.SetRarityRange(SoulManager.instance.rarityRange);

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

        EffectAdderObject eaobj = Instantiate(node.Data.treasureStageData.obj, node.transform.position + new Vector3(0, 1, 0), Quaternion.identity).GetComponent<EffectAdderObject>();
        eaobj.SetRarityRange(SoulManager.instance.rarityRange);

        yield return new WaitForSeconds(1f);

        StartCoroutine(ClearStage());
    }

    StageSetting stageSetting = new StageSetting(0.12f, 0.1f, 0.18f);

    public bool StartScene()
    {
        CurrentStageStartData = FindObjectOfType<StageStartData>();
        if (CurrentStageStartData == null) return false;
        UIManager.instance.BattleUI.OnUI();
        maxStageTurn = CurrentStageStartData.trunCount;
        portal = CurrentStageStartData.bossPortal;
        trunCountText = CurrentStageStartData.trunCountText;
        trunCountText.text = maxStageTurn.ToString();
        clearEliteRoom = 0;
        currentTurn = -1;
        Player.instance.OnPositionSet(CurrentStageStartData.transform.position, CurrentStageStartData.transform.rotation);
        Player.instance.SetupPlayer();
        
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            for (int j = 0; j < CurrentStageStartData.useEnemy.Length; j++)
            {
                if (enemyPrefab[i].name == CurrentStageStartData.useEnemy[j].ToString())
                {
                    enemyPool.Add(CurrentStageStartData.useEnemy[j], new MemoryPool(enemyPrefab[i]));
                    break;
                }
            }
        }
        CurrentStageStartData.startNode.isSetStageData = true;
        stageSetting.ReadNode();
        stageSetting.Setting();

        return true;
        //CurrentStageStartData.startNode.StageDataTrigger(0, 0);
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
        Player.instance.OnPlayerStatReset();
        UIManager.instance.BattleUI.OffUI();
        
        if (stageSpowning != null)
        {
            StopCoroutine(stageSpowning);
        }

        foreach (MemoryPool pool in enemyPool.Values)
        {
            pool.DestroyPool();
        }
        enemyPool.Clear();

        SceneControlManager.instance.LoadScene(SceneName.GameOver);
        InventoryManager.instance.ResetInventory();
        //Player.instance.OnPlayer();
        //Player.instance.OnPositionSet(new Vector3(0, 0, 0), Quaternion.identity);
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

        float random = Random.Range(0f, 1f);

        if (node.Data.stageType == StageType.Elite && random <= eliteRewardsRange[clearEliteRoom++])
        {
            EffectAdderObject eaobj = Instantiate(node.Data.treasureStageData.obj, node.transform.position + new Vector3(0, 1, 0), Quaternion.identity).GetComponent<EffectAdderObject>();
            eaobj.SetRarityRange(SoulManager.instance.rarityRange);
        }

        if (currentTurn >= maxStageTurn)
        {
            //포탈 소환
            portal.transform.position = node.transform.position + new Vector3(0, 0, 21);
            portal.SetActive(true);
            node.door12.SetActive(false);
            CurrentStageStartData.bridges.SetActive(false);
            stageSetting.OffAllRenderer(node.pos);
            
            foreach (MemoryPool pool in enemyPool.Values)
            {
                pool.DestroyPool();
            }
            enemyPool.Clear();

            yield break;
        }

        node.OpenDoor();
    }
}
