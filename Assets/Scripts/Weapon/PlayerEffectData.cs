using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEffectData", menuName = "PlayerEffectData/EffectData")]
public class PlayerEffectData : ScriptableObject
{
    [SerializeField]
    public GameObject EffectPrefab;
    [SerializeField]
    public SoundSetting soundSetting;
}

[System.Serializable]
public struct SoundSetting
{
    public AudioClip audioClip;
    [Range(0, 1)]
    public float soundVolume;
}