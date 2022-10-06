using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIController : CharacterController
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (IndicatorManager.Instance != null)
        {
            IndicatorManager.Instance.OnInScreen(this as CharacterController);
        }
    }

    protected override void Update()
    {
        base.Update();
        MoveHandle();
        AttackHandle();
        InDicatorHandle();
    }

    public override void Init()
    {
        SetAiSkin();
        RandomName();
        base.Init();
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

    private void RandomName()
    {
        var names = GameManager.Instance.DataController.GetNames();
        var randomIndex = UnityEngine.Random.Range(0, names.Length);
        var randomName = names[randomIndex];
        SetCharacterNameUI(randomName,SkinColor);
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
        switch (newState)
        {
            case CharacterState.Idle:
                StartCoroutine(IdleToMove());
                break;
        }
        if (newState != CharacterState.Move)
        {
            if (navMesh.isOnNavMesh)
            {
                navMesh.isStopped = true;
                navMesh.updateRotation = false;
            }
        }
        else
        {
            if (navMesh.isOnNavMesh)
            {
                navMesh.isStopped = false;
                navMesh.updateRotation = true;
            }
        }
        base.OnCharacterChangeState(oldState,newState);
    }

    protected override void OnCharacterDie()
    {
        if (IsInScreen())
        {
            GameAudioManager.Instance.PlayClip(AudioType.CharacterDie,0.5f);
        }
        base.OnCharacterDie();
    
        
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
        var gData = GameManager.Instance.DataController;
        //NOTE: Random weapon 
        var playerOwnWeapon = gData.GetPlayerOwnWeapon();
        var index = UnityEngine.Random.Range(0, playerOwnWeapon.Count);
        var newWp = PollingManager.Instance.WeaponPolling.Instantiate(playerOwnWeapon[index]
            )
            .GetComponent<WeaponController>();
        newWp.Init(gameObject,weaponHolderTF);
        return newWp;
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
                + randomVector3 * UnityEngine.Random.Range(4f, 9f));
        }
    }
    
    protected override void DeSpawn()
    {
        
    }

    private void InDicatorHandle()
    {
        if (!IsInScreen())
        {
            if (CharacterState != CharacterState.Init
                && CharacterState != CharacterState.Dance
                && CharacterState != CharacterState.Die)
            {
                IndicatorManager.Instance.OnOutScreen(this as CharacterController);
            }
        }
        else
        {
            IndicatorManager.Instance.OnInScreen(this as CharacterController);
        }
        
    }

    private bool IsInScreen()
    {
        var screenPoint = CameraController.Instance
            .MainCam.WorldToViewportPoint(CacheComponentManager.Instance.TFCache.Get(gameObject).position);
        if (screenPoint.x >= 1 || screenPoint.x <= 0
                               || screenPoint.y >= 1 || screenPoint.y <= 0)
        {
            return false;
        }
        return true;
    }

    private void SetAiSkin()
    {
        var dataController = GameManager.Instance.DataController;
        
        //SetRandom Hat
        var useHat = UnityEngine.Random.Range(0, 2)>0?true:false;
        if (useHat)
        {
            var hats = dataController.SkinRef[SkinType.Hat].Item;
            var hatIndex = UnityEngine.Random.Range(0, hats.Count);
            SetHat(hats.ElementAt(hatIndex).Value.CharacterObject);
        }
        //
        
        //SetRandom shield
        var useShield = UnityEngine.Random.Range(0, 2)>0?true:false;
        if (useShield)
        {
            var shields = dataController.SkinRef[SkinType.Shield].Item;
            var shieldIndex = UnityEngine.Random.Range(0, shields.Count);
            SetShield(shields.ElementAt(shieldIndex).Value.CharacterObject);
        }
        //        
        
        var usePant=UnityEngine.Random.Range(0, 2)>0?true:false;
        if (usePant)
        {
            var pants = dataController.SkinRef[SkinType.Pant].Item;
            var pantIndex = UnityEngine.Random.Range(0, pants.Count);
            SetPant(pants.ElementAt(pantIndex).Value.CharacterObject.GetComponent<PantInfo>().Material);
        }
        
        
        //random skin color
        skinSkinMesh.material.color = Random.ColorHSV();
        //

    }
}
