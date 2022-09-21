using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public SphereCollider sphereCollider;

    private void Start()
    {
        Debug.Log(TrueRadius());
    }

    public float TrueRadius()
    {
        var tR = Mathf.Abs(transform.lossyScale.x);
        if (tR < Mathf.Abs(transform.lossyScale.y))
        {
            tR = transform.lossyScale.y;
        }
        if (tR < Mathf.Abs(transform.lossyScale.z))
        {
            tR = Mathf.Abs(transform.lossyScale.z);
        }
        return tR * sphereCollider.radius;
    }
}
