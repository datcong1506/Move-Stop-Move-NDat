using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : WeaponController
{
    #region Canxoa
    
    

    #endregion
    protected override void SpawnBullet(Vector3 target)
    {
        // var nBullet = PollingManager.Instance.GetBullet(this);
        var nBullet = InstanBulelt();
        var bulletCCL=CacheComponentManager.Instance.BulletCache.Get(nBullet);
        bulletCCL.Init(
            owner,
            selfTransform.position,
            selfTransform.eulerAngles,
            selfTransform.lossyScale
            ,target);
        
    }
}
