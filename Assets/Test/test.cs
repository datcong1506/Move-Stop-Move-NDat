using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private GameObject kk;

    [ContextMenu("Test")]
    public void Test()
    {
        Instantiate(kk);
        Debug.Log("l");
        Debug.Log("k");
    }
}
