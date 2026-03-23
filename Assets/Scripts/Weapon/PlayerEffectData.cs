using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEffectData", menuName = "PlayerEffectData/EffectData")]
public class PlayerEffectData : ScriptableObject
{
    [SerializeField]
    public GameObject EffectPrefab;
}
