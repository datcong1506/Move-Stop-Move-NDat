using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartUIController : UICanvas
{

    [SerializeField] private Animator selfAnimator;
    private string hideParam = "Hide";
    [SerializeField] private TextMeshProUGUI goldCountTMP;
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

    }
    public void SoundButton()
    {

    }
    public void VibButton()
    {

    }

}
