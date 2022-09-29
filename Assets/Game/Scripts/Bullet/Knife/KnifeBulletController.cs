using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBulletController : BulletController
{
    public override void Init(GameObject owner, Vector3 posision, Vector3 eulerAngle, Vector3 scale, Vector3 target)
    {
        base.Init(owner, posision, eulerAngle, scale, target);
        CacheComponentManager.Instance
            .TFCache.Get(gameObject)
            .LookAt(target,Vector3.left);
    }
}
