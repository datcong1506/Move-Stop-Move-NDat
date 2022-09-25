using System;
[Serializable]
public enum UI
{
    StartUI,
    SkinShopUI,
    WeaponShopUI,
    InGameUI,
    PauseUI,
    SettingMenuUI,
    RevivalUI,
    LoseUI,
    WinUI,
}

public enum SkinType
{
    Hat,
    Pant,
    Shield,
    SkinCombo,
}

[Serializable]
public enum WeaponType:int
{
    Hammer=1,
    Knife=2,
    TripleKnife=3,
    Bommerang=4,
}

[Serializable]
public enum WeaponSkinType
{
    Custom,
    Normal,
}