using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] protected Rigidbody rigidbody;
    [SerializeField] private float lifeTime=4f;
    [SerializeField] private bool destroyWhenHitCharacter;
    public Rigidbody Rigidbody => rigidbody;
    private GameObject owner;
    protected bool isStuckInWall;
    public virtual void Init(
        GameObject owner,
        Vector3 posision,
        Vector3 eulerAngle,
        Vector3 scale,
        Vector3 target)
    {
        isStuckInWall = false;
        this.owner = owner;
        var selfTf = CacheComponentManager.Instance.TFCache.Get(gameObject);
        selfTf.position = posision;
        selfTf.eulerAngles = eulerAngle;
        selfTf.localScale = scale;
        var directToTarget = target - selfTf.position;
        directToTarget.y = 0;
        rigidbody.velocity = (directToTarget).normalized * speed;
        StartCoroutine(DestroySelf());
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        
        Hit(other.gameObject);
        HitWallHandle(other.gameObject);
    }

    private void HitWallHandle(GameObject wall)
    {
        if (CacheComponentManager.Instance.WallCache.TryGet(wall, out var wallController))
        {
            isStuckInWall = true;
            rigidbody.velocity=Vector3.zero;
        }
    }

    
    
    public void Hit(GameObject other)
    {
        if(isStuckInWall) return;
        if (other.gameObject != owner)
        {
            if (CacheComponentManager.Instance.CCCache
                .TryGet(other, out var characterController))
            {
                if (characterController.IsAlive())
                {
                    
                    characterController.OnBeHit(characterController);
                    CacheComponentManager.Instance.CCCache
                        .Get(owner.gameObject)
                        .OnCharacterKillEnemy();
                    if (destroyWhenHitCharacter)
                    {
                        gameObject.SetActive(false);
                        this.StopAllCoroutines();
                    }
                    CacheComponentManager.Instance
                        .BloodEffectPolling.Get(
                            PollingManager.Instance.InstantiateHitCharacterEffect())
                        .Init(CacheComponentManager.Instance.TFCache.Get(gameObject).position);
                }
            }
        }
    }
    
    
}
