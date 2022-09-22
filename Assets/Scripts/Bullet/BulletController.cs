using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rigidbody;
    
    
    public virtual void Init(Transform weapon,Vector3 target)
    {
        var selfTf = CacheComponentManager.Instance.TFCache.Get(gameObject);
        selfTf.position = weapon.position;
        selfTf.rotation = weapon.rotation;
        selfTf.localScale = weapon.lossyScale;
        rigidbody.velocity = (target - selfTf.position).normalized * speed;
    }
    
    
}
