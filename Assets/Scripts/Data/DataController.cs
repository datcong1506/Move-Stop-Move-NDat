using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataController:MonoBehaviour
{
    [SerializeField] private StaticData staticData;
    [SerializeField] private DynamicData dynamicData;
    private Dictionary<ItemType, GameObject[]> itemsPrefabs = new Dictionary<ItemType, GameObject[]>();
    public DynamicData DynamicData
    {
        get
        {
            if (dynamicData == null)
            {
                LoadData();
            }
            return dynamicData;
        }
    }
    private string persistentPath = "";
    private void Awake()
    {
        Init();
        /*
        WriteData();
        */
        LoadData();
    }
    private void Init()
    {
        persistentPath = Application.persistentDataPath
                         +FixVariable.DATA_PATH;
    }
    #region WriteAndReadJsonFile

    private void LoadData()
    {
        if (File.Exists(persistentPath))
        {
            string json = File.ReadAllText(persistentPath);
            DynamicData data = JsonUtility.FromJson<DynamicData>(json);
            this.dynamicData = data;
            Debug.Log(data.OwnHats.Count);
        }
        else
        {
            dynamicData = new DynamicData();
        }
    }

    private void WriteData()
    {
        File.WriteAllText(persistentPath,JsonUtility.ToJson(dynamicData));
    }

    #endregion


    private Dictionary<WeaponType, Item> weapons=new Dictionary<WeaponType, Item>();

    public void GetPlayerWeapon()
    {
        
    }

    public Item GetWeapon(WeaponType weaponType)
    {
        var weapons = GetItems(ItemType.Weapon);
        return weapons[(int)(weaponType)];
    }
    
    
    public Item[] GetItems(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Weapon:
                return staticData.Weapons;
            case ItemType.Pant:
                return staticData.Pants;    
            case ItemType.Hat:
                return staticData.Hats;
            case ItemType.Shield:
                return staticData.Shields;
        }
        return null;
    }
}
