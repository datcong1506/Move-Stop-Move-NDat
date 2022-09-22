using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PollingManager : Singleton<PollingManager>
{
    private Dictionary<WeaponType, ObjectPolling> weaponsPolling = new Dictionary<WeaponType, ObjectPolling>();
    private Dictionary<GameObject, ObjectPolling> bulletPolling = new Dictionary<GameObject, ObjectPolling>();
    public void GetWeapon(WeaponType weaponType)
    {
        /*if (!weaponsPolling.ContainsKey(weaponType))
        {
            weaponsPolling.Add(weaponType,
                new ObjectPolling(gameObject,
                    GameManager.Instance.DataController.get));
        }*/
    }

    public GameObject GetBullet(WeaponController from)
    {
        if (!bulletPolling.ContainsKey(from.gameObject))
        {
            bulletPolling.Add(from.gameObject
                ,new ObjectPolling(gameObject,
                    from.Bullet));
        }

        return bulletPolling[from.gameObject].Instantiate();
    }
    
}
