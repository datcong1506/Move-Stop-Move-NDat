using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PollCache<T> where  T : Object
{
    private Dictionary<T, ObjectPolling> pollCache;
    public PollCache()
    {
        pollCache = new Dictionary<T, ObjectPolling>();
    }
    public GameObject Instantiate(T from)
    {
        if (!pollCache.ContainsKey(from))
        {
            pollCache.Add(from,new ObjectPolling(null,from as GameObject));
        }
        return pollCache[from].Instantiate();
    }
}

public class PollingManager : Singleton<PollingManager>
{
    private PollCache<GameObject> weaponPolling;
    public PollCache<GameObject> WeaponPolling
    {
        get
        {
            if (weaponPolling == null)
            {
                weaponPolling = new PollCache<GameObject>();
            }
            return weaponPolling;
        }
    }
    private PollCache<GameObject> bulletPolling;
    public PollCache<GameObject> BulletPolling
    {
        get
        {
            if (bulletPolling == null)
            {
                bulletPolling = new PollCache<GameObject>();
            }

            return bulletPolling;
        }   
    }

    private ObjectPolling enemyPolling;

    public GameObject GetEnemy()
    {
        if (enemyPolling == null)
        {
            enemyPolling = new ObjectPolling(gameObject, GameManager.Instance.DataController.GetEnemyPrefab(),40);
        }

        if (enemyPolling != null)
        {
            return enemyPolling.Instantiate();
        }
        else
        {
            return null;
        }

    }
}
