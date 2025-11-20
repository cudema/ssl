using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Stage")]
public class StageData : ScriptableObject
{
    [SerializeField]
    GameObject stageFild;
    [SerializeField]
    GameObject[] enemyPrefab;
    [SerializeField]
    EnemyGroup[] enmeyGroup;

    [SerializeField]
    float waveDilayTime;

    [SerializeField]
    StageType stageType;

    public GameObject StageFild
    {
        get => stageFild;
    }

    public GameObject[] EnemyPrefab
    {
        get => enemyPrefab;
    }

    public EnemyGroup[] EnmeyGroup
    {
        get => enmeyGroup;
    }

    public float WaveDilayTime
    {
        get => waveDilayTime;
    }
}