using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkinShopUIController : UICanvas
{
    [SerializeField] private GameObject itemUIPrefab;
    [SerializeField] private Transform itemHolderTransform;
    private List<ItemUIController> skinUIs=new List<ItemUIController>();
    [SerializeField] private TextMeshProUGUI goldCount;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject unEquipButton;
    [SerializeField] private GameObject unLockButton;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private GameObject unlockOneTimeButton;

    [SerializeField] private Transform choseItemEffect;
    [SerializeField] private Transform ChoseGroupEffect;

    private ItemUIController currentChoseItem;
    private SkinType currentSkintype;
    
    [Header("ITemGroups")]
    [SerializeField] private Transform pant;
    [SerializeField] private Transform hat;
    [SerializeField] private Transform shield;
    [SerializeField] private Transform skinCombo;

    

    
    [SerializeField] private Camera shopCam;

    private void Update()
    {
        UpdateGoldCount();
    }

    public override void OnEnter()
    {
        CameraController.Instance.AddCamOverLay(shopCam);
        GameManager.Instance.EnterSkinShop();
        LoadItems(SkinType.Hat);
    }
    public override void OnExit()
    {
        GameManager.Instance.ExitSkinShop();
        CameraController.Instance.RemoveCamOverlay(shopCam);
        Destroy(gameObject);
    }
    private void LoadItems(SkinType skinType)
    {
        //Clear itemUIs
        foreach (var skinUIController in skinUIs)
        {
            Destroy(skinUIController.gameObject);
        }
        skinUIs.Clear();
        //
        currentSkintype = skinType;
        // initialize new list itemUI
        var dataController = GameManager.Instance.DataController;
        var skinUIButtonInfos = dataController.GetSkinUIButtonInfos(skinType);
        foreach (var skinUIButtonInfo in skinUIButtonInfos)
        {
            //Instantiate new ItemUI
            var newItemUIController = Instantiate(itemUIPrefab)
                                        .GetComponent<ItemUIController>();
            newItemUIController.Init(this,itemHolderTransform,skinUIButtonInfo);
            skinUIs.Add(newItemUIController);
        }
        //
        ChoseGroupEffectHandle(skinType);
        //
        //set preview   
        GameManager.Instance.CharacterPreviewController.Reset();
        //
        SelectSkinHandle(skinUIs[0],skinUIs[0].ItemUIButtonInfo);
    }

    private void UpdateButtonHandle(ItemUIButtonInfo itemUIButtonInfo)
    {
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        unLockButton.SetActive(false);
        unlockOneTimeButton.SetActive(false);
        if (itemUIButtonInfo.Equipped)
        {
            unEquipButton.SetActive(true);
            return;
        }
        if (itemUIButtonInfo.Own)
        {
            equipButton.SetActive(true);
            return;
        }
        unLockButton.SetActive(true);
        unlockOneTimeButton.SetActive(true);
        valueText.text = itemUIButtonInfo.Value.ToString();
    }
    
    public void SelectSkinHandle(ItemUIController itemUIController,ItemUIButtonInfo itemUIButtonInfo)
    {
        UpdateButtonHandle(itemUIButtonInfo);
        UpdateChoseSkinEffect(itemUIController.transform);
        currentChoseItem = itemUIController;
        GameManager.Instance.CharacterPreviewController.SetCharacterSkin(currentSkintype,itemUIButtonInfo.SkinName);
    }
    public void SelectSkinHandle(string name)
    {
        for (int i = 0; i < skinUIs.Count; i++)
        {
            var skinUI = skinUIs[i];
            if (skinUI.ItemUIButtonInfo.SkinName == name)
            {
                SelectSkinHandle(skinUI, skinUI.ItemUIButtonInfo);
                break;
            }
        }
    }

    private void UpdateGoldCount()
    {
        goldCount.text = GameManager.Instance.DataController.GoldCount.ToString();
    }
    private void UpdateChoseSkinEffect(Transform target)
    {
        StartCoroutine(DelaySetChoseEffect(target));
    }

    private void ChoseGroupEffectHandle(SkinType skinType)
    {
        switch (skinType)
        {
            case  SkinType.Hat:
                UpdateChoseGroupEffect(hat);
                break;
            case  SkinType.Shield:
                UpdateChoseGroupEffect(shield);
                break;
            case  SkinType.SkinCombo:
                UpdateChoseGroupEffect(skinCombo);
                break;
            case  SkinType.Pant:
                UpdateChoseGroupEffect(pant);
                break;
        }
    }

    [SerializeField] private Transform choseGroupEffect;
    private void UpdateChoseGroupEffect(Transform parent)
    {
        choseGroupEffect.SetParent(parent);
        choseGroupEffect.localPosition=Vector3.zero;
        choseGroupEffect.localScale=Vector3.one;
    }
    

    private IEnumerator DelaySetChoseEffect(Transform target)
    {
        yield return new WaitForEndOfFrame();
        choseItemEffect.SetParent(target);
        choseItemEffect.localScale=Vector3.one;
        choseItemEffect.localPosition=Vector3.zero;
        var posision = choseItemEffect.position;
        choseItemEffect.SetParent(transform);
        choseItemEffect.position = posision;
    }
    

    private void ReLoad(string skinName)
    {
        LoadItems(currentSkintype);
        SelectSkinHandle(skinName);
    }
    
    
    public void UnLockButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        var rs= GameManager.Instance.DataController.UnLockSkin(currentSkintype, currentChoseItem.ItemUIButtonInfo.SkinName);
        if (rs)
        {
            ReLoad(currentChoseItem.ItemUIButtonInfo.SkinName);
        }
    }

    public void ExitButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        UIManager.Instance.LoadUI(UI.StartUI);
    }
    
    // NOTE: Unlock by ads
    public void UnLockOneTimeButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        var rs= GameManager.Instance.DataController.UnLockSkinOneTime(currentSkintype, currentChoseItem.ItemUIButtonInfo.SkinName);
        if (rs)
        {
            ReLoad(currentChoseItem.ItemUIButtonInfo.SkinName);
        }
    }

    public void EquipButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        GameManager.Instance
            .DataController.SetPlayerSkin(currentSkintype,currentChoseItem.ItemUIButtonInfo.SkinName);
        ReLoad(currentChoseItem.ItemUIButtonInfo.SkinName);
    }

    public void UnEquipButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        GameManager.Instance
            .DataController.UnEquipPlayerSkin(currentSkintype);
        ReLoad(currentChoseItem.ItemUIButtonInfo.SkinName);
    }
    
    public void LoadPants()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        LoadItems(SkinType.Pant);
    }
    public void LoadShields()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        LoadItems(SkinType.Shield);
    }
    public void LoadHats()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        LoadItems(SkinType.Hat);
    }
    public void LoadSkinCombo()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        LoadItems(SkinType.SkinCombo);
    }
}
