    using System;
    using System.Collections;
using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

public class PauseUIController : UICanvas
{
    private DataController dataController;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI aliveCount;
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (LevelManager.Instance != null)
        {
            aliveCount.text = GameManager.Instance.CharacterAliveCount.ToString();
        }
    }

    private void Init()
    {
        dataController = GameManager.Instance.DataController;
        SetSound();
        SetSound();
        SetVib();
        SetVib();
    }

    public override void OnEnter()
    {
        
    }
    
    public override void OnExit()
    {
        Destroy(gameObject);
    }

    public void ExitButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        animator.SetBool(FixVariable.OPENSETTING,false);
    }
    public void SettingButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        animator.SetBool(FixVariable.OPENSETTING,true);
    }

    public void HomeButton()
    {
        // reset level
        GameManager.Instance.TryAgain();
        
    }

    public void ContinueButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        animator.SetBool(FixVariable.OPENSETTING,false);
    }
    public void SoundButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        SetSound();
    }

    private void SetSound()
    {
        dataController.UseSound = !dataController.UseSound;
        animator.SetBool(FixVariable.SOUND,dataController.UseSound);
    }

    public void VibButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        SetVib();
    }

    private void SetVib()
    {
        dataController.UseVib = !dataController.UseVib;
        animator.SetBool(FixVariable.VIB,dataController.UseVib);
    }
}
