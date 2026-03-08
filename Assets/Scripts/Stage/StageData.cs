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

    public EnemyGroup[] EnmeyGroup
    {
        get => enmeyGroup;
    }

    public float WaveDilayTime
    {
        get => waveDilayTime;
    }
}