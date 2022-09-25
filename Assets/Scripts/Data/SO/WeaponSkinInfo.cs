using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "WeaponSkin",menuName = "Data/WeaponSkin")]
public class WeaponSkinInfo:ScriptableObject
{
    public int Value;
    public WeaponSkinType WeaponSkinType;
    public GameObject PreviewWeapon;
    public GameObject WorldWeapon;
}