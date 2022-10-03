using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class PlayerController : CharacterController
{

    [Header("Movement")] [SerializeField] private GameObject inputCanvasGO;
    [SerializeField] private PlayerInputController playerInputController;
    private Transform mainCamTransform;
    [SerializeField] private Transform selfTransform;
    private float rotateSpeed;
    [SerializeField] private float rotateSmoothTime = 0.1f;
    private bool canChangeState;

    [SerializeField] private Transform lockTargetTF;
    #region Canxoa

 

    #endregion
    
    
    protected override void Awake()
    {
        base.Awake();
        CameraController.Instance.SetFollowCam(selfTransform,uiTransform);
    }
    
    public override void Init()
    {
        SetPlayerSkin();
        base.Init();
        mainCamTransform = CameraController.Instance.MainCam.transform;
        inputCanvasGO.SetActive(true);
    }

   

    
    protected override WeaponController GetCharacterWeapon()
    {
        var gData = GameManager.Instance.DataController;
        var newWp = PollingManager.Instance.WeaponPolling.Instantiate(gData.GetPlayerWeapon())
            .GetComponent<WeaponController>();
        newWp.Init(gameObject,weaponHolderTF);
        return newWp;
        return null;
    }
    protected override void DeSpawn()
    {
        base.DeSpawn();
        inputCanvasGO.SetActive(false);
    }

    protected override void OnCharacterChangeState(CharacterState oldState, CharacterState newState)
    {
        base.OnCharacterChangeState(oldState, newState);
        
    }
    

    protected override void Update()
    {
        base.Update();
        ChangeStateHandle();
        Move();
        UpdateLockTarget();
    }

    private void UpdateLockTarget()
    {
        if (CharacterState != CharacterState.Die&&CharacterState!=CharacterState.Init)
        {
            if (CanAttack())
            {
                if (!lockTargetTF.gameObject.activeSelf)
                {
                    lockTargetTF.gameObject.SetActive(true);
                }
                target = FindNearestTarget();
                lockTargetTF.position = target;
                //NOTE: if not set rotation, locktarget with rotate with player because it is player child
                // Vector3(90,0,0) just a eulerangle in prefab
                lockTargetTF.rotation=Quaternion.Euler(90,0,0);
            }
            else
            {
                if (lockTargetTF.gameObject.activeSelf)
                {
                    lockTargetTF.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (lockTargetTF.gameObject.activeSelf)
            {
                lockTargetTF.gameObject.SetActive(false);
            }
        }
    }
    
    private void ChangeStateHandle()
    {
        //moveinput
        var playerInput = playerInputController.direc;
        playerInput = playerInput.normalized;
        //
        switch(CharacterState){
            case CharacterState.Idle:
                if (CanAttack())
                {
                    target = FindNearestTarget();
                    CharacterState = CharacterState.Attack;
                    break;
                }
                if (playerInput.magnitude > 0.1f)
                {
                    //transistion to run state
                    CharacterState = CharacterState.Move;
                }
                break;
            case CharacterState.Move:
                if (playerInput.magnitude <= 0.1f)
                {
                    //transistion to idle state
                    CharacterState = CharacterState.Idle;
                }
                break;
            case CharacterState.Attack:
                if (playerInput.magnitude > 0.1f)
                {
                    //transistion to run state
                    CharacterState = CharacterState.Move;
                }
                break;
        }
        
    }
    protected override void Move()
    {
        if(CharacterState!=CharacterState.Move) return;
        
        //player input
        var moveInput = playerInputController.direc;
        moveInput = moveInput.normalized;

        if (moveInput.magnitude > 0.1f)
        {
            var targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + mainCamTransform.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(selfTransform.eulerAngles.y, targetAngle, ref rotateSpeed,
                rotateSmoothTime);
            selfTransform.rotation = Quaternion.Euler(0, angle, 0);
            var moveDirec = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            navMesh.Move(moveDirec * speed * Time.deltaTime);
        }
    }

    protected override void UpdateUI()
    {
        base.UpdateUI();
        if (CharacterState==CharacterState.Init)
        {
            if (inputCanvasGO.gameObject.activeSelf)
            {
                inputCanvasGO.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!inputCanvasGO.gameObject.activeSelf)
            {
                inputCanvasGO.gameObject.SetActive(true);
            }
        }
    }

    protected override void OnCharacterDie()
    {
        GameAudioManager.Instance.PlayClip(AudioType.CharacterDie);
        base.OnCharacterDie();
    }

    public override void OnCharacterKillEnemy()
    {
        base.OnCharacterKillEnemy();
        GameAudioManager.Instance.Vibrate();
        GameManager.Instance.OnPlayerKillEnemy();
    }

    protected override void OnCharacterLevelUp()
    {
        base.OnCharacterLevelUp();
        CacheComponentManager.Instance
            .LevelUpEffect.Get(
                PollingManager.Instance.InstantiateLevelUpEffect()
                ).Init(CacheComponentManager.Instance.TFCache.Get(gameObject));
    }
    

    
    private void SetPlayerSkin()
    {
        var dataController = GameManager.Instance.DataController;
        SetHat(dataController.GetPlayerHat());
        SetShield(dataController.GetPlayerShield());
        SetPant(dataController.GetPlayerPantMaterial());
        SetSkin(dataController.GetPlayerSkinMaterial());
    }
}
