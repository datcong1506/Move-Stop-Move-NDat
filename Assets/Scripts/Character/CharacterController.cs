using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public enum CharacterState
{
    Init,// just do nothing
    Idle,
    Move,
    Dance,
    Attack,
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
    [Header("Skins")] [SerializeField] protected Transform hatHolderTF;
    [SerializeField] protected SkinnedMeshRenderer pantSkinMesh;
    [SerializeField] protected Transform shieldHolderTF;
    [Header("Weapon")] [SerializeField] protected Transform weaponHolderTF;
    protected WeaponType equipWeaponType;
    [Header("Attack")] 
    [SerializeField]protected WeaponController weaponController;
    private Dictionary<Transform,int> targets = new Dictionary<Transform, int>();
    public Vector3 target;
    private CharacterState characterState;
    public CharacterState CharacterState
    {
        get
        {
            return characterState;
        }
        set
        {
            characterState = value;
            ChangeAnimation(value.ToString());
        }
    }


    private int version; // neu character nay da dc hoi sinh lai so voi lan truoc
    public int Version
    {
        get => version;
        set => version = value;
    }

    protected abstract WeaponController GetCharacterWeapon();
    
    
    protected bool CanAttack()
    {
        List<Transform> removeTargets = new List<Transform>();
        
        for (int i = 0; i < targets.Count; i++)
        {
            var targetKeyValue = targets.ElementAt(i);
            var targetController
                = CacheComponentManager.Instance.CCCache.Get(targetKeyValue.Key.gameObject);
            if (!targetController.IsAlive(targetKeyValue.Value))
            {
                removeTargets.Add(targetKeyValue.Key);
            }
        }

        for (int i = 0; i < removeTargets.Count; i++)
        {
            targets.Remove(removeTargets[i]);
        }

        if (targets.Count > 0)
        {
            return true;
        }

        return false;
    }

    private Vector3 FindNearestTarget()
    {
        float maxLengToTarget = 1000;
        Vector3 tPosision = Vector3.zero;
        for (int i = 0; i < targets.Count; i++)
        {
            var targetKeyValue = targets.ElementAt(i);
            var distanceToTarget =
                (CacheComponentManager.Instance.TFCache
                    .Get(gameObject).position - targetKeyValue.Key.position).magnitude;
            if (distanceToTarget < maxLengToTarget)
            {
                tPosision = targetKeyValue.Key.position;
            }
        }
        return tPosision;
    }
    public bool IsAlive(int ver)
    {
        return ver==Version;
    }
    
    public void AddTarget(Transform nTarget)
    {
        if(nTarget.gameObject==gameObject) return;
        if (!targets.ContainsKey(nTarget))
        {
            targets.Add(nTarget,CacheComponentManager.Instance.CCCache.Get(nTarget.gameObject).version);
        }
    }

    public void RemoveTarget(Transform eTarget)
    {
        if (targets.ContainsKey(eTarget))
        {
            targets.Remove(eTarget);
        }
    }
    
    protected abstract void Move();

    public virtual void Attack()
    {
        
    }
    protected abstract void OnBeHit();

    protected virtual void Init()
    {
        Version++;
    }
    protected abstract void DeSpawn();
    protected virtual void ChangeAnimation(string newParam)
    {
        animator.ResetTrigger(currentParam);
        currentParam = newParam;
        animator.SetTrigger(newParam);
    }
}
