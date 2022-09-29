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
    [SerializeField] private int currentWeaponTypeIndex;
    [SerializeField] private WeaponSKinPreviewButtonController currentSkin;

    [Header("Buttons")] 
    [SerializeField] private GameObject customPanel;
    [SerializeField] private GameObject equippedPanel;
    [SerializeField] private GameObject selectButton;
    [SerializeField] private GameObject unlockPanel;
    
    
    
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
        LoadWeapon(currentWeaponTypeIndex);
    }

    private void LoadWeapon(WeaponType weaponType)
    {
        for (int i = 0; i < skinButtonControllers.Count; i++)
        {
            Destroy(skinButtonControllers[i].gameObject);
        }
        skinButtonControllers.Clear();
        var weaponSkins = GameManager.Instance.DataController.GetWeaponSkinButtonInfos(weaponType);
        weaponName.text = weaponType.ToString();
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
            }
            
        }

        ChoseSkin(skinButtonControllers[0]);
    }


    private void LoadWeapon(int index)
    {
        if (index >= 0 && index < weaponTypes.Length)
        {
            currentWeaponTypeIndex = index;
            LoadWeapon(weaponTypes[currentWeaponTypeIndex]);
        }
    }
    public void ExitButton()
    {
        UIManager.Instance.LoadUI(UI.StartUI);
    }

    public void SelectButton()
    {
        GameManager.Instance
            .DataController
            .SetPlayerWeapon(weaponTypes[currentWeaponTypeIndex],currentSkin.WeaponSkinButtonInfo.WeaponSkinName);
    }

    public void UnLockOneTimeButton()
    {
        
    }

    public void UnLockButton()
    {
        // GameManager.Instance.DataController.UnLockSkin()
    }
    
    public void NextLeftButton()
    {
        LoadWeapon(currentWeaponTypeIndex-1);
    }
    public void NextRightButton()
    {
        LoadWeapon(currentWeaponTypeIndex+1);
    }

    private void UpdateActiveButtonHandle(WeaponSkinButtonInfo weaponSkinButtonInfo)
    {
        customPanel.SetActive(false);
        equippedPanel.SetActive(false);
        selectButton.SetActive(false);
        unlockPanel.SetActive(false);

        if (weaponSkinButtonInfo.IsEquipping)
        {
            equippedPanel.SetActive(true);
        }
        else
        {
            if (weaponSkinButtonInfo.Own)
            {
                selectButton.SetActive(true);
            }
            else
            {
                unlockPanel.SetActive(true);
            }
        }
        
    }


    private GameObject currentPreview;
    private void Preview(WeaponSKinPreviewButtonController weaponSKinPreviewButtonController)
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }

        currentPreview = Instantiate(weaponSKinPreviewButtonController.WeaponSkinButtonInfo.PreviewPrefab);
        currentPreview.GetComponent<PreviewObjectController>().Init(previewHolder);
    }
  
        
    public void ChoseSkin(WeaponSKinPreviewButtonController weaponSKinPreviewButtonController)
    {
        currentSkin = weaponSKinPreviewButtonController;
        UpdateActiveButtonHandle(weaponSKinPreviewButtonController.WeaponSkinButtonInfo);
        Preview(weaponSKinPreviewButtonController);
    }
}
