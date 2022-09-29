using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : WeaponController
{
    protected override void SpawnBullet(Vector3 target)
    {
        var bullet = InstanBulelt();
        var bulletCCL=CacheComponentManager.Instance.BulletCache.Get(bullet);
        bulletCCL.Init(
            owner,
            selfTransform.position,
            selfTransform.eulerAngles,
            selfTransform.lossyScale
            ,target);
    }
}
