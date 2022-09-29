using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Events;

public class DataController:MonoBehaviour
{
    [SerializeField] private StaticData staticData;
    [SerializeField] private DynamicData dynamicData;
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
    public UnityEvent<int> OnGoldCountChangeEvent; 
    public int GoldCount
    {
        get
        {
            return dynamicData.GoldCount;
        }
        set
        {
            dynamicData.GoldCount = value;
            OnGoldCountChangeEvent?.Invoke(dynamicData.GoldCount);
        }
    }
    public bool UseSound
    {
        get
        {
            return dynamicData.UseSound;
        }
        set
        {
            dynamicData.UseSound = value;
        }
    }
    public bool UseVib
    {
        get
        {
            return dynamicData.UseVib;
        }
        set
        {
            dynamicData.UseVib = value;
        }
    }
    private string persistentPath
    {
        get
        {
            return Application.persistentDataPath
                   +FixVariable.DATA_PATH;
        }
    }
    private Dictionary<SkinType, ItemRef<SkinInfo>> skinRef;
    public Dictionary<SkinType, ItemRef<SkinInfo>> SkinRef
    {
        get
        {
            if (skinRef == null)
            {
                skinRef = new Dictionary<SkinType, ItemRef<SkinInfo>>();

                skinRef.Add(SkinType.Hat, new ItemRef<SkinInfo>("item/skin/hat"));
                skinRef.Add(SkinType.Pant, new ItemRef<SkinInfo>("item/skin/pant"));
                // :UNDONE!!! need add more skin such as shield, skinCombo
            }

            return skinRef;
        }
    }


    private Dictionary<WeaponType, ItemRef<WeaponSkinInfo>> weaponRef;
    public Dictionary<WeaponType, ItemRef<WeaponSkinInfo>> WeaponRef{
        get
        {
            if (weaponRef == null)
            {
                weaponRef = new Dictionary<WeaponType, ItemRef<WeaponSkinInfo>>();
                weaponRef.Add(WeaponType.Hammer, new ItemRef<WeaponSkinInfo>("item/weapon/"+WeaponType.Hammer.ToString()));
                weaponRef.Add(WeaponType.Knife, new ItemRef<WeaponSkinInfo>("item/weapon/"+WeaponType.Knife.ToString()));
                weaponRef.Add(WeaponType.TripleKnife, new ItemRef<WeaponSkinInfo>("item/weapon/"+WeaponType.TripleKnife.ToString()));

            }
            return weaponRef;
        }
    }

    private void Awake()
    {
        Init();
        LoadData();
    }
    private void OnDestroy()
    {
        OnDeSpawn();
    }

    private void Start()
    {
       // var rs= GetWeaponSkinButtonInfos(WeaponType.Hammer);
       // Debug.Log(rs.Count);
    }

    private void Init()
    {
    }
    #region WriteAndReadJsonFile
    private void LoadData()
    {
        if (File.Exists(persistentPath))
        {
            string json = File.ReadAllText(persistentPath);
            DynamicData data = JsonUtility.FromJson<DynamicData>(json);
            this.dynamicData = data;
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
    [ContextMenu("DeleteData")]
    private void DeleteFile()
    {
        File.Delete(persistentPath);
    }
    #endregion
    
    
    
    
    
    private void OnDeSpawn()
    {
        WriteData();
    }
    public List<ItemUIButtonInfo>  GetSkinUIButtonInfos(SkinType skinType)
    {
        return GetSkinUIButtonInfos(SkinRef[skinType], skinType);
    }
    private List<ItemUIButtonInfo> GetSkinUIButtonInfos(ItemRef<SkinInfo> itemRef,SkinType skinType)
    {
        List<ItemUIButtonInfo> itemUIButtonInfos = new List<ItemUIButtonInfo>();
        var items = itemRef.Item;
        for (int i = 0; i < items.Count; i++)
        {
            var item = items.ElementAt(i);
            itemUIButtonInfos.Add(
                new ItemUIButtonInfo(
                    item.Key,
                    false,
                    IsOwnSkin(skinType,item.Key),
                    item.Value.Value,
                    item.Value.PreviewObject
                )
            );
        }
        return itemUIButtonInfos;
    }

    public bool IsOwnWeaponSkin(string name, WeaponSkinType weaponSkinType)
    {
        if (weaponSkinType == WeaponSkinType.Custom)
        {
            return true;
        }

        if (DynamicData.OwnWeaponSkins.Contains(name))
        {
            return true;
        }

        return false;
    }
    
    public List<WeaponSkinButtonInfo> GetWeaponSkinButtonInfos(WeaponType weaponType)
    {
        List<WeaponSkinButtonInfo> infos = new List<WeaponSkinButtonInfo>();
        var skins = WeaponRef[weaponType].Item;
        for (int i = 0; i < skins.Count; i++)
        {
            var skin = skins.ElementAt(i);
            infos.Add(
                new WeaponSkinButtonInfo
                {
                    WeaponSkinName = skin.Key,
                    Value = skin.Value.Value,
                    PreviewPrefab = skin.Value.PreviewWeapon,
                    Own = IsOwnWeaponSkin(skin.Key,skin.Value.WeaponSkinType),
                    WeaponSkinType = skin.Value.WeaponSkinType,
                    IsEquipping = DynamicData.WeaponSkinName==skin.Key,
                }
            );
        }

        return infos;
    }

    public List<WeaponType> GetWeaponsEnum()
    {
        return new List<WeaponType>
        {
            WeaponType.Hammer,
        };
    }


    private bool IsOwnSkin(SkinType skinType, string skinName)
    {
        var ownSkins = GetOwnSkins(skinType);
        return ownSkins.Contains(skinName);
    }
    private List<string> GetOwnSkins(SkinType skinType)
    {
        switch (skinType)
        {
            case SkinType.Hat:
                return dynamicData.OwnHats;
            case SkinType.Pant:
                return dynamicData.OwnPants;
            case SkinType.Shield:
                return dynamicData.OwnShields;
            case SkinType.SkinCombo:
                return dynamicData.OwnSkinCombo;
            default:
                return null;
        }
    }

    public bool UnLockSkin(SkinType skinType, string name)
    {
        var skins = SkinRef[skinType];
        if (skins.Item[name].Value <= dynamicData.GoldCount)
        {
            dynamicData.GoldCount -= skins.Item[name].Value;
            // UNDONE
            var ownSkins = GetOwnSkins(skinType);
            ownSkins.Add(name);
            //
            return true;
        }

        return false;
    }

    public bool UnLockWeapon(WeaponType weaponType)
    {
        return true;
    }

    public bool UnLockWeaponSkin(WeaponType weaponType, string skinName)
    {
        return true;
    }


    public static T[] GetItemsInRS<T>(string path) where  T : ScriptableObject
    {
        var itemInRS = Resources.LoadAll<T>(path);
        return itemInRS;
    }
    public static void SetItemsInRS<T>(string resourcePath,ref Dictionary<string,T> itemRef) where T : ScriptableObject
    {
        itemRef = new Dictionary<string, T>();
        var itemInRS = GetItemsInRS<T>(resourcePath);
        for (int i = 0; i < itemInRS.Length; i++)
        {
            itemRef.Add(itemInRS[i].name,itemInRS[i]);
        }
    }

    public GameObject GetPlayerWeapon()
    {
        return WeaponRef[dynamicData.WeaponEquipped].Item[dynamicData.WeaponSkinName].WorldWeapon;
    }
    
    public GameObject GetEnemyPrefab()
    {
        return staticData.EnemyPrefab;
    }
    public GameObject GetPlayerPrefab()
    {
        return staticData.PlayerPrefab;
    }

    public Level GetCurrentLevel()
    {
        return GetLevel(DynamicData.CurrentLevelIndex);
    }
    public Level GetLevel(string name)
    {
        var allLVs= Resources.LoadAll<Level>("level/");
        for (int i = 0; i < allLVs.Length; i++)
        {
            if (allLVs[i].name == name)
            {
                return allLVs[i];
            }
        }

        return null;
    }

    public void SetLevel(string lvName)
    {
        DynamicData.CurrentLevelIndex = lvName;
    }

    public void SetPlayerWeapon(WeaponType weaponType, string skin)
    {
        DynamicData.WeaponEquipped = weaponType;
        DynamicData.WeaponSkinName = skin;
    }

    public Material GetPlayerPantMaterial()
    {
        if (DynamicData.IsUsingPant())
        {
            return SkinRef[SkinType.Pant].Item[DynamicData.PantEquipped].Material;
        }

        return null;
    }

    public Material GetPlayerSkinMaterial()
    {
        if (DynamicData.IsUsingSkinCombo())
        {
            return SkinRef[SkinType.SkinCombo].Item[DynamicData.PantEquipped].Material;
        }
        return null;
    }

    public GameObject GetPlayerHat()
    {
        if (DynamicData.IsUsingHat())
        {
            return SkinRef[SkinType.Hat].Item[DynamicData.HatEquipped].CharacterObject;
        }
        return null;
    }
    
    public GameObject GetPlayerShield()
    {
        if (DynamicData.IsUsingShield())
        {
            return SkinRef[SkinType.Shield].Item[DynamicData.ShieldEquipped].CharacterObject;
        }

        return null;
    }
}

//NOTE: this can use for weapon and skin:pant,hat, ...
public class  ItemRef<T> where  T : ScriptableObject
{
    private string path;
    private Dictionary<string, T> item;
    public Dictionary<string, T> Item
    {
        get
        {
            if (item == null)
            {
                DataController.SetItemsInRS(path, ref item);
            }
            return item;
        }   
    }
    public ItemRef(string path)
    {
        this.path = path;
    }
}
