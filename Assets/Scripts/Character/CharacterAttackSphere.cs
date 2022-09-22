using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackSphere : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    private void OnTriggerEnter(Collider other)
    {
        AddTarget(other);
    }
    
    private void OnTriggerExit(Collider other)
    {
       RemoveTarget(other);
    }
    private void TransparentWall(Collider other){}
    private void UnTransparentWall(Collider other){}
    private void RemoveTarget(Collider other)
    {
        if (other.gameObject != characterController.gameObject)
        {
            if (CacheComponentManager.Instance.CCCache.TryGet(other.gameObject, out var controller))
            {
                characterController.RemoveTarget(
                    CacheComponentManager.Instance.TFCache.Get(other.gameObject));
            }
        }
    }
    private void AddTarget(Collider other)
    {
        if (CacheComponentManager.Instance.CCCache.TryGet(other.gameObject, out var controller))
        {
            characterController.AddTarget(
                CacheComponentManager.Instance.TFCache.Get(other.gameObject));
        }
    }
}