using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//NOTE: will refactor this
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
    //NOTE: could move prefab to staticdata
    [SerializeField] private GameObject levelUpEffectPrefab;
    [SerializeField] private GameObject hitCharacterEffectPrefab;
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
    private ObjectPolling levelUpEffectPolling;
    private ObjectPolling hitCharacterEffectPolling;
    public GameObject InstantiateLevelUpEffect()
    {
        if (levelUpEffectPolling == null)
        {
            levelUpEffectPolling = new ObjectPolling(gameObject, levelUpEffectPrefab, 7);
        }

        return levelUpEffectPolling.Instantiate();
    }
    
    //Note: Blood Effect
    public GameObject InstantiateHitCharacterEffect()
    {
        if (hitCharacterEffectPolling == null)
        {
            hitCharacterEffectPolling = new ObjectPolling(gameObject, hitCharacterEffectPrefab, 50);
        }
        return hitCharacterEffectPolling.Instantiate();
    }
    
    // NOTE: Instantiate a Enemy from poll
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
    
    // NOTE: get bulletflying count
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
    
    // NOTE: Disable all bullet is flying when load new level, or reset current level
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
    
    // NOTE: Disable all enemy is in active when load new level, or reset current level
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
