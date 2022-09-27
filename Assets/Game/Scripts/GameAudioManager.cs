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


class AudioConvert
{
    public AudioType AudioType;
    public AudioClip Clip;
}


public class GameAudioManager : MonoBehaviour
{
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


}
