using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinCombo : MonoBehaviour
{
    [SerializeField]public GameObject hatPrefab;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private Material skinPrefab;
    [SerializeField] private Material pantPrefab;

    [SerializeField] private Transform hatHolderTF;
    [SerializeField] private Transform shieldHolderTF;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer pantMeshRenderer;


    private void Start()
    {
        //Init();
    }

    private void Init()
    {
        if (TryGetHat() != null)
        {
            Instantiate(TryGetHat()).GetComponent<CharacterObjectController>().Init(hatHolderTF);
        }
        if (TryGetShield() != null)
        {
            Instantiate(TryGetShield()).GetComponent<CharacterObjectController>().Init(shieldHolderTF);
        }

        if (TryGetPant() != null)
        {
            pantMeshRenderer.sharedMaterial = TryGetPant();
        }

        if (TryGetSkin() != null)
        {
            skinnedMeshRenderer.sharedMaterial = TryGetSkin();
        }
    }
    
    public GameObject TryGetHat()
    {
        return hatPrefab != null ? hatPrefab : null;
    }

    public GameObject TryGetShield()
    {
        return shieldPrefab != null ? shieldPrefab : null;
    }

    public Material TryGetSkin()
    {
        return skinPrefab != null ? skinPrefab : null;
    }

    public Material TryGetPant()
    {
        return pantPrefab != null ? pantPrefab : null;
    }
}
