using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixer audioMixer;

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

    void Start()
    {
        SetBGM(bgm);
        PlayBGM();
        StartSetVolume();
    }

    [SerializeField, Header("오디오 소스")]
    AudioSource bgmSource;
    [SerializeField]
    AudioSource sfxSourece;

    [SerializeField, Header("기본 BGM")]
    SoundSetting bgm;

    public void PlaySFX(SoundSetting soundSetting)
    {
        sfxSourece.volume = soundSetting.soundVolume;
        sfxSourece.PlayOneShot(soundSetting.audioClip);
    }

    public void SetBGM(SoundSetting soundSetting)
    {
        bgmSource.volume = soundSetting.soundVolume;
        bgmSource.clip = soundSetting.audioClip;
    }

    public void PlayBGM()
    {
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Pause();
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("Master", value);
        float tempValue;
        audioMixer.GetFloat("Master", out tempValue);
        PlayerPrefs.SetFloat("MasterVolume", tempValue);
    }

    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGM", value);
        float tempValue;
        audioMixer.GetFloat("BGM", out tempValue);
        PlayerPrefs.SetFloat("BGMVolume", tempValue);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFX", value);
        float tempValue;
        audioMixer.GetFloat("SFX", out tempValue);
        PlayerPrefs.SetFloat("SFXVolume", tempValue);
    }

    void StartSetVolume()
    {
        audioMixer.SetFloat("Master", PlayerPrefs.GetFloat("MasterVolume"));
        audioMixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGMVolume"));
        audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFXVolume"));
    }
}
