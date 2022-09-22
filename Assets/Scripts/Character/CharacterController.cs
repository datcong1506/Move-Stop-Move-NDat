using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum CharacterState
{
    Init,// just do nothing
    Idle,
    Move,
    Dance,
    Die
}

public abstract class CharacterController : StateController
{
    [Header("Movement")]
    [SerializeField] protected float speed = FixVariable.CHARACTER_SPEED;
    [SerializeField] protected NavMeshAgent navMesh;

    [Header("Animation")] 
    [SerializeField] private Animator animator;
    [SerializeField] private string currentParam;
    protected abstract void Move();
    protected abstract void Attack();
    protected abstract void OnBeHit();

    protected abstract void Init();
    protected abstract void DeSpawn();

    protected virtual void ChangeAnimation(string newParam)
    {
        animator.ResetTrigger(currentParam);
        currentParam = newParam;
        animator.SetTrigger(newParam);
    }
}
