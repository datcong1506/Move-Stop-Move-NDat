using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObjectController : MonoBehaviour
{
    [SerializeField] private Vector3 localPosision;
    [SerializeField] private Vector3 localEulerAngle;
    [SerializeField] private Vector3 localScale;
    
    public void Init(Transform parent)
    {
        var selfTransform = transform;
        selfTransform.SetParent(parent);
        selfTransform.localPosition = localPosision;
        selfTransform.localEulerAngles = localEulerAngle;
        selfTransform.localScale = localScale;
    }
}
