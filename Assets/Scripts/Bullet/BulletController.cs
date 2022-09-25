using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] protected Rigidbody rigidbody;

    public Rigidbody Rigidbody => rigidbody;
    private GameObject owner;

    public virtual void Init(
        GameObject owner,
        Vector3 posision,
        Vector3 eulerAngle,
        Vector3 scale,
        Vector3 target)
    {
        this.owner = owner;
        var selfTf = CacheComponentManager.Instance.TFCache.Get(gameObject);
        selfTf.position = posision;
        selfTf.eulerAngles = eulerAngle;
        selfTf.localScale = scale;
        var directToTarget = target - selfTf.position;
        directToTarget.y = 0;
        rigidbody.velocity = (directToTarget).normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {

        Hit(other.gameObject);
    }

    public void Hit(GameObject other)
    {
        if (other.gameObject != owner)
        {
            if (CacheComponentManager.Instance.CCCache
                .TryGet(other, out var characterController))
            {
                if (characterController.IsAlive())
                {
                    characterController.OnBeHit();
                    CacheComponentManager.Instance.CCCache
                        .Get(owner.gameObject)
                        .OnCharacterKillEnemy();
                }
            }
        }
    }
    
    
}
