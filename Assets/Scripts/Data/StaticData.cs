using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class Items
{
    public ItemType ItemType;
    public Item[] Item;
}


[Serializable]
public class Item
{
    public PreviewItem PreviewItem;
    public WorldItem WorldItem;
}

[Serializable]
public class PreviewItem
{
    public GameObject PreviewPrefab;
}

[Serializable]
public class WorldItem
{
    public GameObject WorldPrefab;
}

        [Serializable]
[CreateAssetMenu(fileName = "StaticData", menuName = "Data/StaticData")]
public class StaticData:ScriptableObject
{
    [Header("Weapon")] 
    [SerializeField] private Item[] weapons;
    public Item[] Weapons => weapons;
    [Header("Pant")] 
    [SerializeField] private Item[] pants;
    public Item[] Pants => pants;
    [Header("Hat")] 
    [SerializeField] private Item[] hats;
    public Item[] Hats => hats;
    [Header("Shield")] 
    [SerializeField] private Item[] shields;
    public Item[] Shields => shields;
    [Header("SkinCombo")] [SerializeField]
    private Item[] skinCombos;
    public Item[] SkinCombos => skinCombos;
}
