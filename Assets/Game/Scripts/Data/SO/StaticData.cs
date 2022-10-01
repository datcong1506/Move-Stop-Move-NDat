using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[CreateAssetMenu(fileName = "StaticData", menuName = "Data/StaticData")]
public class StaticData:ScriptableObject
{
    #region FirstData

    // [Header("Weapon")] 
    // [SerializeField] private Item[] weapons;
    // public Item[] Weapons => weapons;
    // [Header("Pant")] 
    // [SerializeField] private Item[] pants;
    // public Item[] Pants => pants;
    // [Header("Hat")] 
    // [SerializeField] private Item[] hats;
    // public Item[] Hats => hats;
    // [Header("Shield")] 
    // [SerializeField] private Item[] shields;
    // public Item[] Shields => shields;
    // [Header("SkinCombo")] [SerializeField]
    // private Item[] skinCombos;
    // public Item[] SkinCombos => skinCombos;

    #endregion
    
    [SerializeField] private GameObject playerPrefab;
    public GameObject PlayerPrefab => playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    public GameObject EnemyPrefab => enemyPrefab;
    [SerializeField] private GameObject characterPreviewPrefab;
    public GameObject CharacterPreviewPrefab => characterPreviewPrefab;

    [SerializeField] private Material defaultPantMaterial;
    public Material DefaultPantMaterial => defaultPantMaterial;
    [SerializeField] private Material defaultSkinMaterial;
    public Material DefaultSkinMaterial => defaultSkinMaterial;
}
