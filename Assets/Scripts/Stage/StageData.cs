using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Stage")]
public class StageData : ScriptableObject
{
    [SerializeField]
    EnemyGroup[] enmeyGroup;

    [SerializeField]
    float waveDilayTime;

    [SerializeField]
    public StageType stageType;

    [SerializeField]
    public int dropCoin;
    [SerializeField]
    public int dropSoul;
    [SerializeField]
    public RestStageData restStageData;
    [SerializeField]
    public ShopStageData shopStageData;
    [SerializeField]
    public TreasureStageData treasureStageData;

    public EnemyGroup[] EnmeyGroup
    {
        get => enmeyGroup;
    }

    public float WaveDilayTime
    {
        get => waveDilayTime;
    }
}

[System.Serializable]
public struct RestStageData
{
    public GameObject obj;
    public float value;
}

[System.Serializable]
public struct ShopStageData
{
    public GameObject obj;
}

[System.Serializable]
public struct TreasureStageData
{
    public GameObject obj;
}