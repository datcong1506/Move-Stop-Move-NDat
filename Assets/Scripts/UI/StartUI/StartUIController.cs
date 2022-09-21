using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUIController : UICanvas
{

    [SerializeField] private Animator selfAnimator;
    private string hideParam = "Hide";

    public override void OnEnter()
    {
        UnHideCanvas();
    }
    public override void OnExit()
    {
        selfAnimator.SetTrigger(hideParam);
    }

    public void HideCanvas()
    {
        gameObject.SetActive(false);
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
