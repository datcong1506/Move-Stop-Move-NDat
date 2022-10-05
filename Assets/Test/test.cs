using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private Material _materiall;

    [ContextMenu("test")]
    public void SetMat()
    {
        _materiall.color=Color.black;
        
    }
}
