using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPreviewController : MonoBehaviour
{

    [SerializeField] private Transform camLookAtTF;
    [SerializeField] private Transform camFollowTF;

    private GameObject currentHat;
    private GameObject currentShield;

    [SerializeField] private Transform selfTransform;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform hatHolderTF;
    [SerializeField] protected SkinnedMeshRenderer pantSkinMesh;
    [SerializeField] protected SkinnedMeshRenderer skinSkinMesh;
    [SerializeField] protected Transform shieldHolderTF;
    [Header("Weapon")] [SerializeField] protected Transform weaponHolderTF;


    

    public void Init(Vector3 posission)
    {
        CameraController.Instance.SetSkinShopCam(camFollowTF,camLookAtTF);
        selfTransform.position = posission;
        AttachWeapon();
        Reset();
    }

    private void AttachWeapon()
    {
        var weapon = Instantiate(GameManager.Instance.DataController.GetPlayerWeapon());
        weapon.GetComponent<WeaponController>().Init(gameObject,weaponHolderTF);
    }
    

    public void Reset()
    {
        var dataController = GameManager.Instance.DataController;
        SetHat(dataController.GetPlayerHat());
        SetShield(dataController.GetPlayerShield());
        SetPant(dataController.GetPlayerPantMaterial());
        SetSkin(dataController.GetPlayerSkinMaterial());
    }

    public void SetCharacterSkin(SkinType skinType, string skinName)
    {
        var characterObject = GameManager.Instance.DataController.GetCharacterSkin(skinType, skinName);
        switch (skinType)
        {
            case SkinType.Hat:
                SetHat(characterObject);
                break;
            case SkinType.Pant:
                SetPant(characterObject
                    .GetComponent<PantInfo>().Material);
                break;
        }
    }

    private void SetHat(GameObject hatPrefab)
    {
        if (currentHat != null)
        {
            Destroy(currentHat);
        }
        if (hatPrefab != null)
        {
            
            currentHat=Instantiate(hatPrefab);
            currentHat.GetComponent<CharacterObjectController>().Init(hatHolderTF);
            //
        }
    }
    private void SetPant(Material pant)
    {
        if (pant != null)
        {
            pantSkinMesh.material = pant;
        }
    }
    private void SetShield(GameObject shieldPrefab)
    {
        if (shieldPrefab != null)
        {
            if (currentShield != null)
            {
                Destroy(currentShield);
            }
            currentShield=Instantiate(shieldPrefab);
            currentShield.GetComponent<CharacterObjectController>().Init(shieldHolderTF);
        }
    }
    private void SetSkin(Material skin)
    {
        if (skin != null)
        {
            skinSkinMesh.material = skin;
        }
    }
  
}
