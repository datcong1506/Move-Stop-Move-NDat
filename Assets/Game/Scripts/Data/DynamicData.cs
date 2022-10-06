using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class DynamicData
{
    public int GoldCount;
    public string CurrentLevelName;
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


    public string PlayerName;
    
    #region GameSetting
    public bool UseSound;
    public bool UseVib;
    #endregion
    public DynamicData()
    {
        GoldCount = 0;
        CurrentLevelName = "Level1";
        BestRank = 99999;

        PlayerName = "ABI";
        
        OwnHats = new List<string>();
        OwnPants = new List<string>();
        OwnShields = new List<string>();
        OwnSkinCombo = new List<string>();
        OwnWeaponSkins = new List<string>{"HammerSkin01"};
        OwnWeapons = new List<string> { WeaponType.Hammer.ToString() };
        
        ResetWeaponEquipped();
        ResetHatEquipped();
        ResetPantEquipped();
        ResetShieldEquipped();
        ResetSkinComboEquipped();
        
        UseSound = true;
        UseVib = true;
    }


    public bool IsUsingHat()
    {
        return HatEquipped != FixVariable.DEFAULT_SKINNAME;
    }

    public bool IsUsingShield()
    {
        return ShieldEquipped != FixVariable.DEFAULT_SKINNAME;
    }

    public bool IsUsingPant()
    {
        return PantEquipped != FixVariable.DEFAULT_SKINNAME;
    }

    public bool IsUsingSkinCombo()
    {
        return SkinComboEquipped != FixVariable.DEFAULT_SKINNAME;
    }

    public void ResetWeaponEquipped()
    {
        WeaponEquipped = WeaponType.Hammer;
        WeaponSkinName = "HammerSkin01";
    }

    public void ResetHatEquipped()
    {
        HatEquipped = FixVariable.DEFAULT_SKINNAME;
    }
    public void ResetPantEquipped()
    {
        PantEquipped = FixVariable.DEFAULT_SKINNAME;
    }
    public void ResetShieldEquipped()
    {
        ShieldEquipped = FixVariable.DEFAULT_SKINNAME;
    }
    public void ResetSkinComboEquipped()
    {
        SkinComboEquipped = FixVariable.DEFAULT_SKINNAME;
    }
}
