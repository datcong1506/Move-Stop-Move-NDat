using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class PlayerController : CharacterController
{
    [SerializeField] private PlayerInputController playerInputController;
    private Transform mainCamTransform;
    [SerializeField] private Transform selfTransform;
    private float rotateSpeed;
    [SerializeField] private float rotateSmoothTime = 0.1f;
    private bool canChangeState;
    private CharacterState playerState;
    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        speed = FixVariable.CHARACTER_SPEED;
        mainCamTransform = CameraController.Instance.MainCam.transform;
        playerState=CharacterState.Idle;
    }

    protected override void DeSpawn()
    {
        
    }

    private void Update()
    {
        Move();
    }

    private void StateActionHandle(){
        switch(playerState){
            case CharacterState.Idle:
                
                break;
        }
    }

    protected override void Move()
    {
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
            ChangeAnimation(FixVariable.RUN_PARAM);
        }
    }

    protected override void Attack()
    {
    }

    protected override void OnBeHit()
    {
        
    }

    
}
