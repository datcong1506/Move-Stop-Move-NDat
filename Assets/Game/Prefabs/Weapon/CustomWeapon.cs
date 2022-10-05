using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWeapon : MonoBehaviour
{
    [SerializeField] private Material topMaterial;
    [SerializeField] private Material bottomMaterial;

    public Material TopMaterial => topMaterial;

    public Material BottomMaterial => bottomMaterial;
}
