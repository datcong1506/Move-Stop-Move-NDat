using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [SerializeField] private Vector3 localOffset;
    [SerializeField] private Vector3 localEulerAngle;
    [SerializeField] private Vector3 localScale;
    [SerializeField] protected Transform selfTransform;
    [SerializeField] protected  GameObject bullet;
    [SerializeField] private float reloadBulletTime = 1.5f;
    [SerializeField] private GameObject render;
    protected GameObject owner;
    public bool IsReady { get; protected set; }
    public GameObject Bullet => bullet;
    public void Init(GameObject owner,Transform holder)
    {
        this.owner = owner;
        selfTransform.SetParent(holder);
        selfTransform.localPosition = localOffset;
        selfTransform.localEulerAngles = localEulerAngle;
        selfTransform.localScale = localScale;
        IsReady = true;
    }
    
    public void Fire(Vector3 target)
    {
        IsReady = false;
        render.SetActive(false);
        SpawnBullet(target);
        StartCoroutine(ReloadCountDown());
    }

    protected abstract void SpawnBullet(Vector3 target);

    protected GameObject InstanBulelt()
    {
        return PollingManager.Instance.BulletPolling.Instantiate(bullet);
    }
    IEnumerator ReloadCountDown()
    {
        yield return new WaitForSeconds(reloadBulletTime);
        IsReady = true;
        render.SetActive(true);
    }
    
    
}
