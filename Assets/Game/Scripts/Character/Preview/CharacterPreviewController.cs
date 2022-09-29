using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPreviewController : MonoBehaviour
{

    private GameObject currentHat;
    private GameObject currentShield;
    
    [SerializeField] protected Transform hatHolderTF;
    [SerializeField] protected SkinnedMeshRenderer pantSkinMesh;
    [SerializeField] protected SkinnedMeshRenderer skinSkinMesh;
    [SerializeField] protected Transform shieldHolderTF;
    [Header("Weapon")] [SerializeField] protected Transform weaponHolderTF;


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        AttachWeapon();
    }

    private void AttachWeapon()
    {
        var weapon = Instantiate(GameManager.Instance.DataController.GetPlayerWeapon());
        weapon.GetComponent<WeaponController>().Init(gameObject,weaponHolderTF);
    }
    
    
    protected void SetSkinHanle(GameObject hatPrefab, GameObject shieldPrefab, Material pant, Material skin)
    {
        if (hatPrefab != null)
        {
            if (currentHat != null)
            {
                Destroy(currentHat);
            }
            currentHat=Instantiate(hatPrefab);
            currentHat.GetComponent<CharacterObjectController>().Init(hatHolderTF);
            //
        }
        if (shieldPrefab != null)
        {
            if (currentShield != null)
            {
                Destroy(currentShield);
            }
            currentShield=Instantiate(shieldPrefab);
            currentShield.GetComponent<CharacterObjectController>().Init(shieldHolderTF);
        }
        if (pant != null)
        {
            pantSkinMesh.material = pant;
        }
        if (skin != null)
        {
            skinSkinMesh.material = skin;
        }
    }
}
