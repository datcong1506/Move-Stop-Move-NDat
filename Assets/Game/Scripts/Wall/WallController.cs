using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material normalMat;
    [SerializeField] private Material hideMat;

    private void Awake()
    {
        Init();
    }

    private void OnDestroy()
    {
        OnDeSpawn();
    }

    private void Init()
    {
        if (CacheComponentManager.Instance != null)
        {
            CacheComponentManager.Instance.WallCache.Add(gameObject);
        }
    }

    private void OnDeSpawn()
    {
        if (CacheComponentManager.Instance != null)
        {
            CacheComponentManager.Instance.WallCache.Remove(gameObject);
        }    }

    public void OnPlayerEnter()
    {
        meshRenderer.material = hideMat;
    }

    public void OnPlayerExit()
    {
        meshRenderer.material = normalMat;
    }
    
}
