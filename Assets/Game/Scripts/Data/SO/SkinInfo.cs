using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Skin",menuName = "Data/Skin")]
public class SkinInfo : ScriptableObject
{
    public int Value; 
    public GameObject PreviewObject;
    public GameObject CharacterObject;
}
