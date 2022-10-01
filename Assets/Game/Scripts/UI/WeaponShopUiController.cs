using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponShopUiController : UICanvas
{
    [SerializeField] private TextMeshProUGUI goldCount;
    
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
    [SerializeField] private GameObject unlockWeaponButton;
    
    private GameObject currentPreview;

    
    private void Awake()
    {
        skinButtonControllers = new List<WeaponSKinPreviewButtonController>();
    }

    private void Update()
    {
        UpdateGoldCount();
    }

    public override void OnEnter()
    {
        CameraController.Instance.AddCamOverLay(shopCam);
        GameManager.Instance.EnterWeaponShop();
        Init();
    }
    public override void OnExit()
    {
        CameraController.Instance.RemoveCamOverlay(shopCam);
        GameManager.Instance.ExitWeaponShop();
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

    private void Exit()
    {
        UIManager.Instance.LoadUI(UI.StartUI);
    }
    
    
    public void ExitButton()
    {
        Exit();
    }

    public void SelectButton()
    {
        GameManager.Instance
            .DataController
            .SetPlayerWeapon(weaponTypes[currentWeaponTypeIndex],currentSkin.WeaponSkinButtonInfo.WeaponSkinName);
        Exit();
    }

    public void UnLockWeaponSkinOneTimeButton()
    {
        GameManager.Instance.DataController.UnlockWeaponSkinOneTime(weaponTypes[currentWeaponTypeIndex],
            currentSkin.WeaponSkinButtonInfo.WeaponSkinName);
        LoadWeapon(weaponTypes[currentWeaponTypeIndex]);
    }

    public void UnlockWeaponSkinButton()
    {
        // GameManager.Instance.DataController.UnLockSkin()
        GameManager.Instance.DataController.UnLockWeaponSkin(weaponTypes[currentWeaponTypeIndex],
            currentSkin.WeaponSkinButtonInfo.WeaponSkinName);
        LoadWeapon(weaponTypes[currentWeaponTypeIndex]);
    }


    public void UnLockWeaponButton()
    {
        GameManager.Instance.DataController.UnLockWeapon(weaponTypes[currentWeaponTypeIndex]);
        LoadWeapon(weaponTypes[currentWeaponTypeIndex]);
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
        unlockWeaponButton.SetActive(false);

        if (!GameManager.Instance.DataController.IsOwnWeapon(weaponTypes[currentWeaponTypeIndex]))
        {
            unlockWeaponButton.SetActive(true);
        }
        else
        {
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
    }


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
    
    
    private void UpdateGoldCount()
    {
        goldCount.text = GameManager.Instance.DataController.GoldCount.ToString();
    }
}
