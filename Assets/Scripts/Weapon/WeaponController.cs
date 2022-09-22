using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Vector3 localOffset;
    [SerializeField] private Vector3 localEulerAngle;
    [SerializeField] private Vector3 localScale;
    [SerializeField] private Transform selfTransform;
    [SerializeField] private  GameObject bullet;
    public GameObject Bullet => bullet;
    public void Init(Transform holder)
    {
        selfTransform.SetParent(holder);
        selfTransform.localPosition = localOffset;
        selfTransform.localEulerAngles = localEulerAngle;
        selfTransform.localScale = localScale;
    }

    public void Fire(Vector3 target)
    {
        var nBullet = PollingManager.Instance.GetBullet(this);
        var bulletCCL=CacheComponentManager.Instance.BulletCache.Get(nBullet);
        bulletCCL.Init(selfTransform,target);
    }
}
