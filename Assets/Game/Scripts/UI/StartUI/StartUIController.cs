using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartUIController : UICanvas
{

    [SerializeField] private Animator selfAnimator;
    private string hideParam = "Hide";
    [SerializeField] private TextMeshProUGUI goldCountTMP;
    [Header("Vibrate")] 
    [SerializeField]private GameObject vibrateOn;
    [SerializeField] private GameObject vibrateOff;
    [Header("Sound")] 
    [SerializeField] private GameObject soundOn;
    [SerializeField] private GameObject soundOff;
    [Header("Ads")] [SerializeField] private GameObject adsOn;
    [SerializeField] private GameObject adsOff;
    [SerializeField] private TextMeshProUGUI rankText;
    private void Update()
    {
        UpdateGoldCount();
    }
    
    public override void OnEnter()
    {
        UnHideCanvas();
        Init();
    }
    public override void OnExit()
    {
        selfAnimator.SetBool(hideParam,true);
    }

    public void Init()
    {
        SoundTrigger();
        SoundTrigger();
        
        VibTrigger();
        VibTrigger();

        SetRankText();
    }

    private void SetRankText()
    {
        var rank = GameManager.Instance.DataController.DynamicData.BestRank > 1000
            ? "Unknown"
            : GameManager.Instance.DataController.DynamicData.BestRank.ToString();
        rankText.text = GameManager.Instance.Level.name+" - Best:# "+ rank;
    }
    
    private void UpdateGoldCount()
    {
        goldCountTMP.text = GameManager.Instance.DataController.GoldCount.ToString();
    }

    //Call By animation Event
    public void HideCanvas()
    {
        Destroy(gameObject);
    }

    public void UnHideCanvas()
    {
        gameObject.SetActive(true);
    }
    
    public void PlayButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        var gManager = GameManager.Instance;
        if (gManager != null)
        {
            gManager.Play();
        }
    }
    public void SkinShopButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        var uiManager = UIManager.Instance;
        if (uiManager != null)
        {
            uiManager.LoadUI(UI.SkinShopUI);
        }
    }
    public void WeaponShopButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        UIManager.Instance.LoadUI(UI.WeaponShopUI);
    }
    public void AdsButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        AdsTrigger();
    }

    private void AdsTrigger()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        adsOff.gameObject.SetActive(adsOn.gameObject.activeSelf);
        adsOn.gameObject.SetActive(!adsOff.gameObject.activeSelf);
    }
    public void SoundButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        SoundTrigger();
    }

    private void SoundTrigger()
    {
        GameManager.Instance.DataController.UseSound = !GameManager.Instance.DataController.UseSound;
        soundOn.SetActive(GameManager.Instance.DataController.UseSound);
        soundOff.SetActive(!GameManager.Instance.DataController.UseSound);
    }
    public void VibButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        VibTrigger();
    }

    private void VibTrigger()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        GameManager.Instance.DataController.UseVib = !GameManager.Instance.DataController.UseVib;
        vibrateOn.SetActive(GameManager.Instance.DataController.UseVib);
        vibrateOff.SetActive(!GameManager.Instance.DataController.UseVib);
    }

}
