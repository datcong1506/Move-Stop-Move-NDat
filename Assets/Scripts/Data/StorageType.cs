using System;
[Serializable]
public enum UI
{
    StartUI,
    SkinShopUI,
    WeaponShopUI,
}

[Serializable]
public enum ItemType
{
    Weapon,
    Hat,
    Pant,
    Shield,
    SkinCombo,
}

[Serializable]
public enum HatType
{
    
}
[Serializable]
public enum PantType
{
    
}
[Serializable]
public enum SkinComboType
{
    
}
[Serializable]
public enum ShieldType
{
    Vabranium,
    UnBroken,
}
[Serializable]
public enum WeaponType:int
{
    Hammer=1,
    Knife=2,
    TripleKnife=3,
    Bommerang=4,
}