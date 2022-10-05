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
            return DynamicData.GoldCount;
        }
        set
        {
            DynamicData.GoldCount = value;
            OnGoldCountChangeEvent?.Invoke(DynamicData.GoldCount);
        }
    }
    public bool UseSound
    {
        get
        {
            return DynamicData.UseSound;
        }
        set
        {
            DynamicData.UseSound = value;
        }
    }
    public bool UseVib
    {
        get
        {
            return DynamicData.UseVib;
        }
        set
        {
            DynamicData.UseVib = value;
        }
    }
    private string persistentPath
    {
        get
        {
            return Application.dataPath
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
                skinRef.Add(SkinType.Shield, new ItemRef<SkinInfo>("item/skin/shield"));
                skinRef.Add(SkinType.SkinCombo, new ItemRef<SkinInfo>("item/skin/skincombo"));

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

    
    
    private List<string> weaponSkinUnLockOneTime=new List<string>();
    private Dictionary<SkinType, string> skinUnlockOneTime = new Dictionary<SkinType, string>();


    private void Awake()
    {
        Init();
        LoadData();
    }
    private void OnDestroy()
    {
        OnDeSpawn();
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
            dynamicData = data;
        }
        else
        {
            dynamicData = new DynamicData();
        }
    }
    private void WriteData()
    {
        File.WriteAllText(persistentPath,JsonUtility.ToJson(DynamicData));
    }
    [ContextMenu("DeleteData")]
    private void DeleteFile()
    {
        File.Delete(persistentPath);
    }
    #endregion
    
    
    
    
    
    private void OnDeSpawn()
    {
        DeleteUnlockOneTime();
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
                    IsSkinEquipped(skinType,item.Key),
                    IsOwnSkin(skinType,item.Key),
                    item.Value.Value,
                    item.Value.PreviewObject
                )
            );
        }
        return itemUIButtonInfos;
    }

    public bool IsSkinEquipped(SkinType skinType, string skinName)
    {
        switch (skinType)
        {
            case SkinType.Hat:
                return DynamicData.HatEquipped == skinName;
            case SkinType.Pant:
                return DynamicData.PantEquipped == skinName;
            case SkinType.Shield:
                return DynamicData.ShieldEquipped == skinName;
            case SkinType.SkinCombo:
                return DynamicData.SkinComboEquipped == skinName;
        }

        return false;
    }

    public bool IsOwnWeapon(WeaponType weaponType)
    {
        return DynamicData.OwnWeapons.Contains(weaponType.ToString());
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
                return DynamicData.OwnHats;
            case SkinType.Pant:
                return DynamicData.OwnPants;
            case SkinType.Shield:
                return DynamicData.OwnShields;
            case SkinType.SkinCombo:
                return DynamicData.OwnSkinCombo;
            default:
                return null;
        }
    }

    public bool UnLockSkin(SkinType skinType, string skinName)
    {
        var skins = SkinRef[skinType];
        if (skins.Item[skinName].Value 
            
            <= DynamicData.GoldCount)
        {
            dynamicData.GoldCount -= skins.Item[skinName].Value;
            // UNDONE
            var ownSkins = GetOwnSkins(skinType);
            ownSkins.Add(skinName);
            //
            return true;
        }

        return false;
    }

    public bool UnLockSkinOneTime(SkinType skinType, string skinName)
    {
        var skins = SkinRef[skinType];
        var ownSkins = GetOwnSkins(skinType);
        ownSkins.Add(skinName);
        skinUnlockOneTime.Add(skinType,skinName);
        //
        return true;
    }
    
    
    public bool UnLockWeapon(WeaponType weaponType)
    {
        if (!DynamicData.OwnWeapons.Contains(weaponType.ToString()))
        {
            DynamicData.OwnWeapons.Add(weaponType.ToString());
        }
        return true;
    }

    public bool UnLockWeaponSkin(WeaponType weaponType, string skinName)
    {
        if (!DynamicData.OwnWeaponSkins.Contains(skinName))
        {
            if (GoldCount >= WeaponRef[weaponType].Item[skinName].Value)
            {
                DynamicData.OwnWeaponSkins.Add(skinName);
                return true;
            }
        }
        return false;
    }
    public bool UnlockWeaponSkinOneTime(WeaponType weaponType, string skinName)
    {
        if (!DynamicData.OwnWeaponSkins.Contains(skinName))
        {
            DynamicData.OwnWeaponSkins.Add(skinName);
            weaponSkinUnLockOneTime.Add(skinName);
            return true;
        }
        return false;
    }
    private void DeleteUnlockOneTime()
    {
        for (int i = 0; i < weaponSkinUnLockOneTime.Count; i++)
        {
            DynamicData.OwnWeaponSkins.Remove(weaponSkinUnLockOneTime[i]);
            if (weaponSkinUnLockOneTime[i] == DynamicData.WeaponSkinName)
            {
                DynamicData.ResetWeaponEquipped();
            }
        }

        for (int i = 0; i < skinUnlockOneTime.Count; i++)
        {
            GetOwnSkins(skinUnlockOneTime.ElementAt(i).Key).Remove(skinUnlockOneTime.ElementAt(i).Value);
            switch (skinUnlockOneTime.ElementAt(i).Key)
            {
                case SkinType.Hat:
                    if (skinUnlockOneTime.ElementAt(i).Value == DynamicData.HatEquipped)
                    {
                        DynamicData.ResetHatEquipped();
                    }
                    break;
                case SkinType.Pant:
                    if (skinUnlockOneTime.ElementAt(i).Value == DynamicData.PantEquipped)
                    {
                        DynamicData.ResetPantEquipped();
                    }
                    break;
                case SkinType.Shield:
                    if (skinUnlockOneTime.ElementAt(i).Value == DynamicData.ShieldEquipped)
                    {
                        DynamicData.ResetShieldEquipped();
                    }
                    break;
                case SkinType.SkinCombo:
                    if (skinUnlockOneTime.ElementAt(i).Value == DynamicData.SkinComboEquipped)
                    {
                        DynamicData.ResetSkinComboEquipped();
                    }
                    break;
            }
        }
    }

    // NOTE:get item like hat,pant in world not in preview.
    public GameObject GetCharacterSkin(SkinType skinType, string skinName)
    {
        return SkinRef[skinType].Item[skinName].CharacterObject;
    }

    public GameObject GetPlayerWeapon()
    {
        return WeaponRef[DynamicData.WeaponEquipped].Item[DynamicData.WeaponSkinName].WorldWeapon;
    }

    //NOTE: undone
    public List<GameObject> GetPlayerOwnWeapon()
    {
        for (int i = 0; i < DynamicData.OwnWeapons.Count; i++)
        {
        }

        return null;
    }
    
    public string GetPlayerName()
    {
        return DynamicData.PlayerName;
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
        return GetLevel(DynamicData.CurrentLevelName);
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
        DynamicData.CurrentLevelName = lvName;
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
        
        return staticData.DefaultPantMaterial;
    }
    
    public Material GetPlayerSkinMaterial()
    {
        return staticData.DefaultSkinMaterial;
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

    public GameObject GetPlayerSKknCombo()
    {
        if (DynamicData.IsUsingSkinCombo())
        {
            return SkinRef[SkinType.SkinCombo].Item[DynamicData.SkinComboEquipped].CharacterObject;
        }
        return null;
    }
    

    public GameObject GetCharacterPreviewPrefab()
    {
        return staticData.CharacterPreviewPrefab;
    }
    
    // NOTE: Change Player Skin : Hat, shield, pant, skinColor.  change mesh undone

    public void SetPlayerSkin(SkinType skin, string skinName)
    {
        switch (skin)
        {
            case SkinType.Hat:
                DynamicData.HatEquipped = skinName;
                /*
                DynamicData.ResetSkinComboEquipped();
                */
                break;
            case SkinType.Pant:
                DynamicData.PantEquipped = skinName;
                /*
                DynamicData.ResetSkinComboEquipped();
                */
                break;
            case SkinType.Shield:
                DynamicData.ShieldEquipped = skinName;
                /*
                DynamicData.ResetSkinComboEquipped();
                */
                break;
            case SkinType.SkinCombo:
                DynamicData.SkinComboEquipped = skinName;
                DynamicData.ResetHatEquipped();
                DynamicData.ResetPantEquipped();
                DynamicData.ResetShieldEquipped();
                break;
        }
    }

    public void UnEquipPlayerSkin(SkinType skin)
    {
        switch (skin)
        {
            case SkinType.Hat:
                DynamicData.ResetHatEquipped();
                break;
            case SkinType.Pant:
                DynamicData.ResetPantEquipped();
                break;
            case SkinType.Shield:
                DynamicData.ResetShieldEquipped();
                break;
            case SkinType.SkinCombo:
                DynamicData.ResetSkinComboEquipped();
                break;
        }
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
