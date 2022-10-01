using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffectController : MonoBehaviour
{
    [SerializeField] private float timeToLife = 1f;
    
    public void Init(Vector3 posision)
    {
        CacheComponentManager.Instance.TFCache.Get(gameObject).position = posision;
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(timeToLife);
        gameObject.SetActive(false);
    }
}
