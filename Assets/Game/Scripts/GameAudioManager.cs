using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum AudioType
{
    CharacterDie,
    Click,
    WeaponHit,
    Lose,
    Win,
    Congra,
    CountDown,
    HammerThrow,
    SizeUp,
}


[Serializable]
class AudioConvert
{
    public AudioType AudioType;
    public AudioClip Clip;
}


[RequireComponent(typeof(AudioSource))]
public class GameAudioManager : Singleton<GameAudioManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioConvert> audioList;
    private Dictionary<AudioType, AudioClip> audioDic=null;
    public Dictionary<AudioType, AudioClip> AudioDic
    {
        get
        {
            if (audioDic == null)
            {
                ConvertAudioListToDic();
            }

            return audioDic;
        }
    }
    private void ConvertAudioListToDic()
    {
        audioDic = new Dictionary<AudioType, AudioClip>();
        for (int i = 0; i < audioList.Count; i++)
        {
            audioDic.Add(audioList[i].AudioType,audioList[i].Clip);
        }
    }


    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnLoadSceneEvent;
    }

    private void OnLoadSceneEvent(Scene arg0, LoadSceneMode arg1)
    {
        audioSource.Stop();
    }

    public void PlayClip(AudioType clipType)
    {
        if (GameManager.Instance.DataController.UseSound)
        {
            //
            audioSource.volume = 1;
            audioSource.Stop();
            audioSource.PlayOneShot(AudioDic[clipType]);
        }
    }
    public void PlayClip(AudioType clipType,float volume)
    {
        if (GameManager.Instance.DataController.UseSound)
        {
            //
            audioSource.volume = volume;
            audioSource.Stop();
            audioSource.PlayOneShot(AudioDic[clipType]);
        }
    }

    public void Vibrate()
    {
        if (GameManager.Instance.DataController.UseVib)
        {
            Handheld.Vibrate();
        }
    }
    
    
    
}
