using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public enum CharacterState
{
    Init,// just do nothing
    Idle,
    Move,
    Dance,
    Attack,
    Die
}

public abstract class CharacterController : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private Transform uiTransform;
    [SerializeField] private Transform uiPanelTransform;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private Image killImage;
    [SerializeField] private TextMeshProUGUI killCountText;
    [Header("Movement")]
    [SerializeField] protected float speed = FixVariable.CHARACTER_SPEED;
    [SerializeField] protected NavMeshAgent navMesh;
    [Header("Animation")] 
    [SerializeField] private Animator animator;
    [SerializeField] private string currentParam;
    [Header("Skins")] [SerializeField] protected Transform hatHolderTF;
    [SerializeField] protected SkinnedMeshRenderer pantSkinMesh;
    [SerializeField] protected SkinnedMeshRenderer skinSkinMesh;
    [SerializeField] protected Transform shieldHolderTF;
    [Header("Weapon")] [SerializeField] protected Transform weaponHolderTF;
    protected WeaponType equipWeaponType;
    [Header("Attack")] 
    [SerializeField]protected WeaponController weaponController;
    protected Dictionary<Transform,int> targets = new Dictionary<Transform, int>();
    public Vector3 target;
    [SerializeField] private int killCount;
    
    [SerializeField]private CharacterState characterState;
    public CharacterState CharacterState
    {
        get
        {
            return characterState;
        }
        set
        {
            var oldState = characterState;
            characterState = value;
            OnCharacterChangeState(oldState,characterState);
        }
    }
    private int version; // neu character nay da dc hoi sinh lai so voi lan truoc
    public int Version
    {
        get => version;
        set => version = value;
    }

    protected virtual void Awake()
    {
    }

    
    

    protected virtual void OnDestroy()
    {
        if (CacheComponentManager.Instance!= null)
        {
            CacheComponentManager.Instance.CCCache.Remove(gameObject);
        }
    }

    protected virtual void Update()
    {
        //
        int a;
    }
    protected void LateUpdate()
    {
        UpdateUIPosision();
    }
    
    protected virtual void OnCharacterChangeState(CharacterState oldState,CharacterState newState)
    {
        ChangeAnimation(newState.ToString());
        switch (newState)
        {
            case CharacterState.Attack:
                RotateToTarget(target);
                break;
        }
    }
    
    protected abstract WeaponController GetCharacterWeapon();
    protected bool CanAttack()
    {
        if (weaponController != null)
        {
            if (!weaponController.IsReady)
            {
                return false;
            }
        }
        
        
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
    protected Vector3 FindNearestTarget()
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

    public bool IsAlive()
    {
        return gameObject.activeSelf && CharacterState != CharacterState.Die;
    }
    public void AddTarget(Transform nTarget)
    {
        Debug.Log(gameObject.name+nTarget.gameObject.name);
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
        if (CharacterState != CharacterState.Die
            && CharacterState!=CharacterState.Init)
        {
            if (weaponController != null)
            {
                weaponController.Fire(target);
            }
            CharacterState = CharacterState.Idle;
        }
    }

    private void RotateToTarget(Vector3 targetPosision)
    {
        var selfTransform =
            CacheComponentManager.Instance.TFCache
                .Get(gameObject);
        selfTransform.LookAt(targetPosision,Vector3.up);
    }

    public virtual void OnBeHit()
    {
        if (CharacterState != CharacterState.Die)
        {
            CharacterState = CharacterState.Die;
        }
    }
    protected virtual void Init()
    {
        //default
        speed = FixVariable.CHARACTER_SPEED;
        Version++;
        targets.Clear();
        killCount = 0;
        CharacterState=CharacterState.Idle;
        CacheComponentManager.Instance.CCCache.Add(gameObject);
        weaponController = GetCharacterWeapon();
    }
    protected abstract void DeSpawn();
    protected virtual void ChangeAnimation(string newParam)
    {
        animator.ResetTrigger(currentParam);
        currentParam = newParam;
        animator.SetTrigger(newParam);
    }
    protected void UpdateCharacterNameUI(string name, Color color)
    {
        characterNameText.text = name;
        killImage.color = color;
    }
    protected void UpdateKillCountUI(int killCount)
    {
        killCountText.text = killCount.ToString();
    }
    protected void UpdateUIPosision()
    {
        var mainCam = CameraController.Instance.MainCam;
        uiPanelTransform.position=mainCam.WorldToScreenPoint(uiTransform.position);
    }
}
