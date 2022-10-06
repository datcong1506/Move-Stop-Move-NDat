using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WeaponShopUiController : UICanvas
{
    [SerializeField] private TextMeshProUGUI goldCount;
    [SerializeField] private GameObject choseSkinMask;
    
    
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private Transform previewHolder;
    [SerializeField] private Transform skinPreviewHolder;
    [SerializeField] private GameObject weaponSkinContainerPrefab;
    [Header("Runtime")]
    [SerializeField] private WeaponType[] weaponTypes;
    [SerializeField] private List<WeaponSKinPreviewButtonController> skinButtonControllers;
    [SerializeField] private CustomWeaponSkinButtonController customWeaponSkinButtonController;
    [SerializeField] private Camera shopCam;
    [Header("Runtime")]
    [SerializeField] private int currentWeaponTypeIndex;
    [Header("Runtime")]
    [SerializeField] private WeaponSKinPreviewButtonController currentSkin;

    [Header("Buttons")] 
    [SerializeField] private GameObject customPanel;
    [SerializeField] private GameObject equippedPanel;
    [SerializeField] private GameObject selectButton;
    [SerializeField] private GameObject unlockPanel;
    [SerializeField] private TextMeshProUGUI unLockText;
    [SerializeField] private GameObject unlockWeaponButton;
    [SerializeField] private GameObject nextLeftButton;
    [SerializeField] private GameObject nextRightButton;
    [Header("Runtime")]
    private GameObject currentPreview;



    [SerializeField] private GameObject choseColorPanel;
    [SerializeField] private ColorPickerTriangle colorPickerTriangle;
    
    
    
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
        weaponTypes = new WeaponType[GameManager.Instance.DataController.WeaponRef.Count];
        for (int i = 0; i < weaponTypes.Length; i++)
        {
            weaponTypes[i] = GameManager.Instance.DataController.WeaponRef.ElementAt(i).Key;
        }
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
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        Exit();
    }
    // Set Player weapon 
    public void SelectButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        GameManager.Instance
            .DataController
            .SetPlayerWeapon(weaponTypes[currentWeaponTypeIndex],currentSkin.WeaponSkinButtonInfo.WeaponSkinName);
        Exit();
    }
    public void UnLockWeaponSkinOneTimeButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        GameManager.Instance.DataController.UnlockWeaponSkinOneTime(weaponTypes[currentWeaponTypeIndex],
            currentSkin.WeaponSkinButtonInfo.WeaponSkinName);
        LoadWeapon(weaponTypes[currentWeaponTypeIndex]);
    }
    public void UnlockWeaponSkinButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        // GameManager.Instance.DataController.UnLockSkin()
        GameManager.Instance.DataController.UnLockWeaponSkin(weaponTypes[currentWeaponTypeIndex],
            currentSkin.WeaponSkinButtonInfo.WeaponSkinName);
        LoadWeapon(weaponTypes[currentWeaponTypeIndex]);
    }
    public void UnLockWeaponButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        GameManager.Instance.DataController.UnLockWeapon(weaponTypes[currentWeaponTypeIndex]);
        LoadWeapon(weaponTypes[currentWeaponTypeIndex]);
    }
    public void NextLeftButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        LoadWeapon(currentWeaponTypeIndex-1);
    }
    public void NextRightButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        LoadWeapon(currentWeaponTypeIndex+1);
    }
    private void UpdateActiveButtonHandle(WeaponSkinButtonInfo weaponSkinButtonInfo)
    {
        customPanel.SetActive(false);
        equippedPanel.SetActive(false);
        selectButton.SetActive(false);
        unlockPanel.SetActive(false);
        unlockWeaponButton.SetActive(false);
        choseSkinMask.SetActive(false);
        
        nextLeftButton.SetActive(true);
        nextRightButton.SetActive(true);

        if (currentWeaponTypeIndex <= 0)
        {
            nextLeftButton.SetActive(false);
        }

        if (currentWeaponTypeIndex >= weaponTypes.Length - 1)
        {
            nextRightButton.SetActive(false);
        }
        
        if (!GameManager.Instance.DataController.IsOwnWeapon(weaponTypes[currentWeaponTypeIndex]))
        {
            unlockWeaponButton.SetActive(true);
            choseSkinMask.SetActive(true);
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
                    unLockText.text = currentSkin.WeaponSkinButtonInfo.Value.ToString();
                }
            }
        }

        if (currentSkin as CustomWeaponSkinButtonController != null)
        {
            customPanel.SetActive(true);
        }
    }

    
    
    public void RotatePreviewHandle(Vector2 delta)
    {
        if (currentPreview != null)
        {
            var previewTransform = currentPreview.transform;
            previewTransform.Rotate(new Vector3(delta.y,delta.x,0));

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
        //
    }
    
    private void UpdateGoldCount()
    {
        goldCount.text = GameManager.Instance.DataController.GoldCount.ToString();
    }
}
