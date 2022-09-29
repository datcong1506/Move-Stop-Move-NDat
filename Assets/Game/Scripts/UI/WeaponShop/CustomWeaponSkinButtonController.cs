using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWeaponSkinButtonController : WeaponSKinPreviewButtonController
{
    public override void Init(WeaponShopUiController weaponShopUiController, WeaponSkinButtonInfo weaponSkinButtonInfo, Transform parent)
    {
        if (preview != null)
        {
            Destroy(preview);
        }
        base.Init(weaponShopUiController, weaponSkinButtonInfo, parent);
    }
}
