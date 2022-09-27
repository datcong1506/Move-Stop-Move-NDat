using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponShopUiController : UICanvas
{
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private Transform previewHolder;
    [SerializeField] private Transform skinPreviewHolder;
    [SerializeField] private GameObject weaponSkinContainerPrefab;

    [SerializeField] private WeaponType[] weaponTypes;
    [SerializeField] private List<WeaponSKinPreviewButtonController> skinButtonControllers;
    [SerializeField] private CustomWeaponSkinButtonController customWeaponSkinButtonController;
    [SerializeField] private Camera shopCam;

    private void Awake()
    {
        skinButtonControllers = new List<WeaponSKinPreviewButtonController>();
    }

    public override void OnEnter()
    {
        GameManager.Instance.IsInSHop = true;
        CameraController.Instance.AddCamOverLay(shopCam);
        Init();
    }
    public override void OnExit()
    {
        CameraController.Instance.RemoveCamOverlay(shopCam);
        GameManager.Instance.IsInSHop = false;
        Destroy(gameObject);
    }
    private void Init()
    {
        LoadWeapon(WeaponType.Hammer);
    }

    private void LoadWeapon(WeaponType weaponType)
    {
        
        for (int i = 0; i < skinButtonControllers.Count; i++)
        {
            Destroy(skinButtonControllers[i].gameObject);
        }
        skinButtonControllers.Clear();
        var weaponSkins = GameManager.Instance.DataController.GetWeaponSkinButtonInfos(weaponType);

        for (int i = 0; i < weaponSkins.Count; i++)
        {
            var weaponSkin = weaponSkins[i];
            if (weaponSkin.WeaponSkinType == WeaponSkinType.Custom)
            {
                customWeaponSkinButtonController.Init(this,weaponSkin,skinPreviewHolder);
            }
            else
            {
                var newWeaponSkinPreviewButtonCCL = Instantiate(weaponSkinContainerPrefab)
                    .GetComponent<WeaponSKinPreviewButtonController>();
                newWeaponSkinPreviewButtonCCL.Init(this,weaponSkin,skinPreviewHolder);
                skinButtonControllers.Add(newWeaponSkinPreviewButtonCCL);
                Debug.Log(weaponSkins.Count);

            }
            
        }
    }
    


    public void SelectButton()
    {
        
    }

    public void UnLockOneTimeButton()
    {
        
    }

    public void UnLockButton()
    {
        
    }
    
    public void NextLeftButton()
    {
        
    }
    public void NextRightButton()
    {
        
    }


    public void ChoseSkin(WeaponSKinPreviewButtonController eaponSKinPreviewButtonController)
    {
        
    }
    
}
