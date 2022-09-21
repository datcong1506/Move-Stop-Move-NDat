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
    public List<SkinType> OwnSkins;
    public List<PantType> OwnPants;
    public List<ShieldType> OwnShields;
}
