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
    StageType stageType;

    [SerializeField]
    public int dropCoin;
    [SerializeField]
    public RestStageData restStageData;

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