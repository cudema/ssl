using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    void Awake()
    {
        // 싱글톤 초기화
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField, Header("오디오 소스")]
    AudioSource bgmSource;
    [SerializeField]
    AudioSource sfxSourece;

    public void PlaySFX(SoundSetting soundSetting)
    {
        sfxSourece.volume = soundSetting.soundVolume;
        sfxSourece.PlayOneShot(soundSetting.audioClip);
    }
}
