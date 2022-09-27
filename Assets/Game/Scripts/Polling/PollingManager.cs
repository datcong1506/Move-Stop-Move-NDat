using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Dictionary<T, ObjectPolling> GetPoll()
    {
        return pollCache;
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

    private ObjectPolling enemyPolling=null;
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


    public int GetBulletCount()
    {
        var bulletPoll=BulletPolling.GetPoll();
        int count = 0;
        for (int i = 0; i < bulletPoll.Count; i++)
        {
            count += bulletPoll.ElementAt(i).Value.UnAvaibaleObjects.Count;
        }
        return count;
    }

    public void DisableAllBullet()
    {
        var bulletPoll=BulletPolling.GetPoll();
        for (int i = 0; i < bulletPoll.Count; i++)
        {
            for (int j = 0; j < bulletPoll.ElementAt(i).Value.UnAvaibaleObjects.Count; j++)
            {
                bulletPoll.ElementAt(i).Value.UnAvaibaleObjects.ElementAt(j).Key.SetActive(false);
            }
        }
    }

    public void DisableAllEnemy()
    {
        if(enemyPolling==null) return;
        var enemyPolls = enemyPolling.Polls;
        for (int i = 0; i < enemyPolls.Count; i++)
        {
            enemyPolls[i].SetActive(false);
        }
        
    }
}