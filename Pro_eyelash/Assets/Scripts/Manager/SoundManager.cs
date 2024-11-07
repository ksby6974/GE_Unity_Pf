using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using chataan.Scripts.Data;
using chataan.Scripts.Enums;
using System;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource buttonSource;
    [SerializeField] private List<SoundProfileData> soundProfileDataList;

    private Dictionary<AudioActionType, SoundProfileData> _audioDict = new Dictionary<AudioActionType, SoundProfileData>();

    // 오버라이딩
    private new void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(gameObject);

            for (int i = 0; i < Enum.GetValues(typeof(AudioActionType)).Length; i++)
                _audioDict.Add((AudioActionType)i, soundProfileDataList.FirstOrDefault(x => x.AudioType == (AudioActionType)i));
        }
    }

    // 오디오 클립 재생용
    public void PlayMusic(AudioClip audioclip)
    {
        if (!audioclip)
        {
            return;
        }

        musicSource.clip = audioclip;
        musicSource.Play();
    }

    // 효과음 재생용
    public void PlayMusic(AudioActionType type)
    {
        var clip = _audioDict[type].GetRandomClip();
        if (clip)
        {
            PlayMusic(clip);
        }
    }

    public void PlayOneShotButton(AudioActionType type)
    {
        var clip = _audioDict[type].GetRandomClip();
        if (clip)
        {
            PlayOneShotButton(clip);
        }
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (clip)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayOneShotButton(AudioClip clip)
    {
        if (clip)
            buttonSource.PlayOneShot(clip);
    }
}
