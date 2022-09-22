using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class DynamicData
{
    public int GoldCount;
    public int CurrentLevelIndex;
    public int BestRank;
    public List<HatType> OwnHats;
    public List<SkinComboType> OwnSkins;
    public List<PantType> OwnPants;
    public List<ShieldType> OwnShields;
    public HatType HatEquipped;
    public int HatSkinID;
    public SkinComboType SkinComboEquipped;
    public int SkinComboID;
    public PantType PantEquipped;
    public int PantSkinID;
    public ShieldType ShieldEquipped;
    public int ShieldSkinID;
    public WeaponType WeaponEquipped;
    public int WeaponSkinID;
    
    public DynamicData()
    {
        GoldCount = 0;
        CurrentLevelIndex = 1;
        BestRank = 99999;
        OwnHats = new List<HatType>();
        OwnPants = new List<PantType>();
        OwnShields = new List<ShieldType>();
    }
}
