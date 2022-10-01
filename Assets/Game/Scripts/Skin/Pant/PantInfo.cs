using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantInfo : MonoBehaviour
{
    // NOTE: can change meshrenderer to material, but there is more drag and drop =))
    [SerializeField] private MeshRenderer meshRenderer;
    public Material Material
    {
        get
        {
            return meshRenderer.sharedMaterial;
        }
    }
}
