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
            // aliveCount.text=LevelManager.Instance.LevelSetting.AICount.ToString();
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
        animator.SetBool(FixVariable.OPENSETTING,false);
    }
    public void SettingButton()
    {
        animator.SetBool(FixVariable.OPENSETTING,true);
    }

    public void HomeButton()
    {
        // reset level
    }

    public void ContinueButton()
    {
        animator.SetBool(FixVariable.OPENSETTING,false);
    }
    public void SoundButton()
    {
        SetSound();
    }

    private void SetSound()
    {
        dataController.UseSound = !dataController.UseSound;
        animator.SetBool(FixVariable.SOUND,dataController.UseSound);
    }

    public void VibButton()
    {
        SetVib();
    }

    private void SetVib()
    {
        dataController.UseVib = !dataController.UseVib;
        animator.SetBool(FixVariable.VIB,dataController.UseVib);
    }
}
