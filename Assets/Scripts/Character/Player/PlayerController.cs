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
    
    #region Canxoa

 

    #endregion
    
    
    protected override void Awake()
    {
        base.Awake();
        CameraController.Instance.SetFollowCam(selfTransform,uiTransform);
    }
    
    protected override void Init()
    {
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
   
}
