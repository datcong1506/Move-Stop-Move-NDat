using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHammerController : BulletController
{
    public override void Init(GameObject owner,Vector3 posision, Vector3 eulerAngle, Vector3 scale, Vector3 target)
    {
        base.Init(owner,posision, eulerAngle, scale, target);
        transform.eulerAngles = new Vector3(90, 0, 90);
        if (CacheComponentManager.Instance.CCCache.TryGet(owner, out var t))
        {
            if (t as PlayerController != null)
            {
                GameAudioManager.Instance.PlayClip(AudioType.HammerThrow);
            }
        }
    }

    private void Update()
    {
        if (!isStuckInWall)
        {
            CacheComponentManager.Instance.TFCache.Get(gameObject)
                .Rotate(new Vector3(0,-500*Time.deltaTime,0),Space.World);
        }
    }
}
