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
        goldCountTMP.text = GameManager.Instance.DataController.GoldCount.ToString();
        
        VibTrigger();
        VibTrigger();
        
        VibTrigger();
        VibTrigger();
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
        var gManager = GameManager.Instance;
        if (gManager != null)
        {
            gManager.Play();
        }
    }
    public void SkinShopButton()
    {
        var uiManager = UIManager.Instance;
        if (uiManager != null)
        {
            uiManager.LoadUI(UI.SkinShopUI);
        }
    }
    public void WeaponShopButton()
    {
        UIManager.Instance.LoadUI(UI.WeaponShopUI);
    }
    public void AdsButton()
    {
        AdsTrigger();
    }

    private void AdsTrigger()
    {
        adsOff.gameObject.SetActive(adsOn.gameObject.activeSelf);
        adsOn.gameObject.SetActive(!adsOff.gameObject.activeSelf);
    }
    public void SoundButton()
    {
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
        VibTrigger();
    }

    private void VibTrigger()
    {
        GameManager.Instance.DataController.UseVib = !GameManager.Instance.DataController.UseVib;
        vibrateOn.SetActive(GameManager.Instance.DataController.UseVib);
        vibrateOff.SetActive(!GameManager.Instance.DataController.UseVib);
    }

}
