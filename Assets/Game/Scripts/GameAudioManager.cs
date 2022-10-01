using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AudioType
{
    CharacterDie,
    Click,
    WeaponHit,
    Lose,
    Win,
    Congra,
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

    public void PlayClip(AudioType clipType)
    {
        if (GameManager.Instance.DataController.UseSound)
        {
            //
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
