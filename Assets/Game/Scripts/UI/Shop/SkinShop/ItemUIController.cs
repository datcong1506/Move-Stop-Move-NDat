using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIButtonInfo
{
    private string skinName;
    public string SkinName => skinName;
    private bool equipped;
    public bool Equipped => equipped;
    private bool own;
    public bool Own => own;
    private int value;
    public int Value => value;
    private GameObject previewPrefab;
    public GameObject PreviewPrefab => previewPrefab;
    public ItemUIButtonInfo(string skinName,bool equipped,bool own,int value,GameObject previewPrefab)
    {
        this.skinName = skinName;
        this.equipped = equipped;
        this.own = own;
        this.value = value;
        this.previewPrefab = previewPrefab;
    }
    
}

public class ItemUIController:MonoBehaviour
{
    private ItemUIButtonInfo itemUIButtonInfo;
    public ItemUIButtonInfo ItemUIButtonInfo => itemUIButtonInfo;
    [SerializeField] private Transform previewHolder;
    private SkinShopUIController skinShopUIController;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject equippedText;
    public void Init(SkinShopUIController skinShopUIController,Transform parent,ItemUIButtonInfo itemUIButtonInfo)
    {
        this.skinShopUIController = skinShopUIController;
        this.itemUIButtonInfo = itemUIButtonInfo;
        transform.SetParent(parent);
        transform.localScale=Vector3.one;

        var objectPreviewController = Instantiate(itemUIButtonInfo.PreviewPrefab)
            .GetComponent<PreviewObjectController>();
        objectPreviewController.Init(previewHolder);
        
        lockIcon.SetActive(!itemUIButtonInfo.Own);
        equippedText.SetActive(itemUIButtonInfo.Equipped);
    }

    

    public void ChoseButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        skinShopUIController.SelectSkinHandle(this,itemUIButtonInfo);
    }
}
