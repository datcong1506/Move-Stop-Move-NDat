using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class DynamicData
{
    public int GoldCount;
    public string CurrentLevelIndex;
    public int BestRank;
    public List<string> OwnHats;
    public List<string> OwnSkinCombo;
    public List<string> OwnPants;
    public List<string> OwnShields;
    public List<string> OwnWeaponSkins;
    public List<string> OwnWeapons;
    public string HatEquipped;
    public string SkinComboEquipped;
    public string PantEquipped;
    public string ShieldEquipped;
    public WeaponType WeaponEquipped;
    public string WeaponSkinName;


    #region GameSetting
    public bool UseSound;
    public bool UseVib;
    #endregion
    public DynamicData()
    {
        GoldCount = 0;
        CurrentLevelIndex = "Level1";
        BestRank = 99999;
        OwnHats = new List<string>();
        OwnPants = new List<string>();
        OwnShields = new List<string>();
        OwnWeaponSkins = new List<string>();
        OwnWeapons = new List<string> { WeaponType.Hammer.ToString() };
        WeaponEquipped = WeaponType.Hammer;
        WeaponSkinName = "HammerSkin01";
        UseSound = true;
        UseVib = true;
    }
}
