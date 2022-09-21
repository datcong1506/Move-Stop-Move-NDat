using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class Skin
{
    public GameObject PreviewPrefab;
    public GameObject SkinPrefab;
    public int Value;
    public string Des;
}

[Serializable]
public class Weapon
{
    public GameObject WeaponPrefab;
    public GameObject PreviewPrefab;
}


[Serializable]
[CreateAssetMenu(fileName = "StaticData", menuName = "Data/StaticData")]
public class StaticData:ScriptableObject
{
    [Header("UIPrefab")]
    [SerializeField] private GameObject startUIPrefab;
    [SerializeField] private GameObject skinShopUIPrefab;
    [SerializeField] private GameObject weaponShopUIPrefab;

    [Header("Skin")]
    [SerializeField] private Skin[] Skins;
    [Header("Hat")]
    [SerializeField] private Skin[] Hats;
    [Header("Pant")]
    [SerializeField] private Skin[] Pants;
    [Header("Shield")]
    [SerializeField] private Skin[] Shields;

    public GameObject GetUIPrefab(UI uIId)
    {
        GameObject rs = null;
        switch (uIId)
        {
            case UI.StartUI:
                rs = startUIPrefab;
                break;
            case UI.SkinShopUI:
                rs = skinShopUIPrefab;
                break;
        }
        return rs;
    }
}
