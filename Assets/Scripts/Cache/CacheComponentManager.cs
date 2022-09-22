using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheComponent<T>
{
    private Dictionary<GameObject, T> cache;

    public CacheComponent()
    {
        cache = new Dictionary<GameObject, T>();
    }

    public T Get(GameObject from)
    {
        if (!cache.ContainsKey(from))
        {
            cache.Add(from,from.GetComponent<T>());
        }

        return cache[from];
    }
}

public class CacheComponentManager : Singleton<CacheComponentManager>
{
    public CacheComponent<Transform> TFCache=new CacheComponent<Transform>();
    public CacheComponent<CharacterController> CCCache = new CacheComponent<CharacterController>();
    public CacheComponent<BulletController> BulletCache = new CacheComponent<BulletController>();
}
