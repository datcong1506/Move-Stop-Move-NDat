using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : CharacterController
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    protected override void Update()
    {
        MoveHandle();
        AttackHandle();
    }

    private void MoveHandle()
    {
        if (CharacterState == CharacterState.Move)
        {
            var selfTransform = CacheComponentManager.Instance.TFCache.Get(gameObject);
            if ((selfTransform.position-navMesh.destination).magnitude<0.1f)
            {
                CharacterState = CharacterState.Idle;
            }
            
        }
    }
    private void AttackHandle()
    {
        if (CharacterState==CharacterState.Idle
            || CharacterState==CharacterState.Move)
        {
            if (CanAttack())
            {
                target = FindNearestTarget();
                CharacterState = CharacterState.Attack;
            }
        }
    }

    protected override void OnCharacterChangeState(CharacterState oldState, CharacterState newState)
    {
        base.OnCharacterChangeState(oldState,newState);
        switch (newState)
        {
            case CharacterState.Idle:
                StartCoroutine(IdleToMove());
                break;
            case CharacterState.Attack:
                break;
        }

        if (newState != CharacterState.Move)
        {
            navMesh.isStopped = true;
        }
        else
        {
            navMesh.isStopped = false;
        }
    }
    private IEnumerator IdleToMove()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 3f));
        if (CharacterState == CharacterState.Idle)
        {
            CharacterState = CharacterState.Move;
            Move();
        }
    }
    
    protected override WeaponController GetCharacterWeapon()
    {
        return null;
    }
    protected override void Move()
    {
        if (CharacterState == CharacterState.Move)
        {
            // move
            var randomDirec = UnityEngine.Random.insideUnitCircle;
            var randomVector3 = new Vector3(randomDirec.x, 0, randomDirec.y).normalized;
            navMesh.SetDestination(
                CacheComponentManager.Instance.TFCache.Get(gameObject).position 
                + randomVector3 * UnityEngine.Random.Range(1f, 9f));
        }
    }
    
    protected override void DeSpawn()
    {
        
    }
    
}
