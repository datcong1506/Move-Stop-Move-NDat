using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleKnifeWeaponController : WeaponController
{
    protected override void SpawnBullet(Vector3 target)
    {
        var midBullet =CacheComponentManager.Instance.BulletCache.Get(InstanBulelt());
        var leftBullet = CacheComponentManager.Instance.BulletCache.Get(InstanBulelt());
        var rightBullet = CacheComponentManager.Instance.BulletCache.Get(InstanBulelt());
        
        midBullet.Init(
            owner,
            selfTransform.position,
            selfTransform.eulerAngles,
            selfTransform.lossyScale
            ,target);

        var direcToTarget = target - selfTransform.position;
        // NOTE tiep tuyen
        var ttDirecToTarget = new Vector3(direcToTarget.z, 0,-direcToTarget.x);
        
        var leftTarget = target+direcToTarget.magnitude*ttDirecToTarget.normalized*Mathf.Tan(0.2f);
        var rightTarget = target * 2 - leftTarget;
        leftBullet.Init(
            owner,
            selfTransform.position,
            selfTransform.eulerAngles,
            selfTransform.lossyScale,
            leftTarget
            );
        rightBullet.Init(
            owner,
            selfTransform.position,
            selfTransform.eulerAngles,
            selfTransform.lossyScale,
            rightTarget
        );
    }
}
