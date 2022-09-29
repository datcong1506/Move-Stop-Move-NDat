using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponSkinButtonInfo
{
    public string WeaponSkinName;
    public int Value;
    public GameObject PreviewPrefab;
    public bool Own;
    public WeaponSkinType WeaponSkinType;
    public bool IsEquipping;
}

public class WeaponSKinPreviewButtonController : MonoBehaviour
{
    [SerializeField] private Transform previewContainer;
    private WeaponSkinButtonInfo weaponSkinButtonInfo;
    public WeaponSkinButtonInfo WeaponSkinButtonInfo => weaponSkinButtonInfo;
    private WeaponShopUiController weaponShopUiController;
    protected GameObject preview;
    public virtual void Init(WeaponShopUiController weaponShopUiController,WeaponSkinButtonInfo weaponSkinButtonInfo,Transform parent)
    {
        this.weaponSkinButtonInfo = weaponSkinButtonInfo;
        this.weaponShopUiController = weaponShopUiController;
        transform.SetParent(parent);
        transform.localScale=Vector3.one;
        
        var previewWeaponController = Instantiate(weaponSkinButtonInfo.PreviewPrefab)
            .GetComponent<PreviewObjectController>();
        previewWeaponController.Init(previewContainer);
        preview = previewWeaponController.gameObject;
    }
    
    public void ChoseButton()
    {
        weaponShopUiController.ChoseSkin(this);
    }
}
