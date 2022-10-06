using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackSphere : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    private void FixedUpdate()
    {
        characterController.RemoveTarget();
    }
    //
    // private void OnEnable()
    // {
    //     StartCoroutine(RemoveTarget());
    // }
    //
    // private void OnDisable()
    // {
    //     StopAllCoroutines();
    // }

    IEnumerator RemoveTarget()
    {
        var waitFoEndOfRame = new WaitForEndOfFrame();
        while (gameObject.activeSelf)
        {
            yield return waitFoEndOfRame;
            characterController.RemoveTarget();

        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }
    
    protected virtual void OnTriggerExit(Collider other)
    {
        
    }
    
    private void OnTriggerStay(Collider other)
    {
        AddTarget(other.gameObject);
    }
    
    private void AddTarget(GameObject other)
    {
        if (CacheComponentManager.Instance.CCCache.TryGet( other,out var controller))
        {
            characterController.AddTarget(CacheComponentManager.Instance.TFCache.Get(other));
        }
    }
}
