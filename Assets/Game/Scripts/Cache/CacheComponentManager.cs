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

    public bool Contain(GameObject key)
    {
        return cache.ContainsKey(key);
    }
    public bool TryGet(GameObject from, out T t)
    {
        if (!cache.ContainsKey(from))
        {
            t = default(T);
            return false;
        }

        t = cache[from];
        return true;
    }

    public bool Add(GameObject from)
    {
        if (!cache.ContainsKey(from))
        {
            cache.Add(from,from.GetComponent<T>());
            return true;
        }
        return false;
    }

    public bool Remove(GameObject from)
    {
        if (cache.ContainsKey(from))
        {
            cache.Remove(from);
            return true;
        }
        return false;
    }

    public void Clear()
    {
        cache.Clear();
    }
}
public class CacheComponentManager : Singleton<CacheComponentManager>
{
    public CacheComponent<Transform> TFCache=new CacheComponent<Transform>();
    public CacheComponent<CharacterController> CCCache = new CacheComponent<CharacterController>();
    public CacheComponent<BulletController> BulletCache = new CacheComponent<BulletController>();
    public CacheComponent<WallController> WallCache = new CacheComponent<WallController>();
    public CacheComponent<PlayerLevelUpEffectController> LevelUpEffect =
        new CacheComponent<PlayerLevelUpEffectController>();
    public CacheComponent<BloodEffectController> BloodEffectPolling = new CacheComponent<BloodEffectController>();

    public void ClearDataOnLoadLevel()
    {
        WallCache.Clear();
        TFCache.Clear();
    }
    
}
