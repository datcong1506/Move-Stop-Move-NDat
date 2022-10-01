using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUpEffectController : MonoBehaviour
{
    [SerializeField]private float t;
    
    private Transform _follow;
    private Transform _transform;
    public void Init(Transform from){
        _transform=transform;
        _transform.localScale = from.lossyScale;
        _follow=from;
        Debug.Log("that la v l");
    }

    private void OnEnable()
    {
        StartCoroutine(DestroySelf());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void LateUpdate(){
        FollowPlayer();
    }

    private void FollowPlayer(){
        Debug.Log("wtf");
        if(_follow!=null){
            _transform.position=_follow.position;
        }
    }

    IEnumerator DestroySelf(){
        yield return new WaitForSeconds(t);
        gameObject.SetActive(false);
    }
}
