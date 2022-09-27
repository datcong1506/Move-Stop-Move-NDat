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

    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject unEquipButton;
    [SerializeField] private GameObject unLockButton;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private GameObject unlockOneTimeButton;

    [SerializeField] private Transform choseItemEffect;
    [SerializeField] private Transform ChoseGroupEffect;

    private ItemUIController currentChoseItem;
    private SkinType currentSkintype;
    
    
    [SerializeField] private Camera shopCam;
    
    public override void OnEnter()
    {
        CameraController.Instance.AddCamOverLay(shopCam);
        GameManager.Instance.IsInSHop = true;
        LoadItems(SkinType.Hat);
        SelectSkinHandle(skinUIs[0],skinUIs[0].ItemUIButtonInfo);
    }
    public override void OnExit()
    {
        CameraController.Instance.RemoveCamOverlay(shopCam);
        GameManager.Instance.IsInSHop = false;
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
        Debug.Log(skinUIButtonInfos.Count);
        foreach (var skinUIButtonInfo in skinUIButtonInfos)
        {
            //Instantiate new ItemUI
            var newItemUIController = Instantiate(itemUIPrefab)
                                        .GetComponent<ItemUIController>();
            newItemUIController.Init(this,itemHolderTransform,skinUIButtonInfo);
            skinUIs.Add(newItemUIController);
            //
        }
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
    private void UpdateChoseSkinEffect(Transform target)
    {
        
        Debug.Log("s");
        StartCoroutine(DelaySetChoseEffect(target));
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
        var rs= GameManager.Instance.DataController.UnLockSkin(currentSkintype, currentChoseItem.ItemUIButtonInfo.SkinName);
        if (rs)
        {
            ReLoad(currentChoseItem.ItemUIButtonInfo.SkinName);
        }
    }
    
    public void UnLockOneTimeButton()
    {
        
    }

    public void EquipButton()
    {
        
    }

    public void UnEquipButton()
    {
        
    }
    
    public void LoadPants()
    {
        LoadItems(SkinType.Pant);
    }
    public void LoadShields()
    {
        LoadItems(SkinType.Shield);
    }
    public void LoadHats()
    {
        LoadItems(SkinType.Hat);
    }
    public void LoadSkinCombo()
    {
        LoadItems(SkinType.SkinCombo);
    }
}
