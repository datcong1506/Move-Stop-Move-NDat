using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    [SerializeField] protected Transform uiTransform;
    [SerializeField] private Transform uiPanelTransform;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private Image killImage;
    [SerializeField] private TextMeshProUGUI killCountText;
    [Header("Movement")]
    [SerializeField] protected float speed = FixVariable.CHARACTER_SPEED;
    [SerializeField] protected NavMeshAgent navMesh;
    public NavMeshAgent NavMesh => navMesh;
    [Header("Animation")] 
    [SerializeField] private Animator animator;
    [SerializeField] private string currentParam;
    [Header("Skins")] [SerializeField] protected Transform hatHolderTF;
    [SerializeField] protected SkinnedMeshRenderer pantSkinMesh;
    [SerializeField] protected SkinnedMeshRenderer skinSkinMesh;
    public Color SkinColor
    {
        get
        {
            return skinSkinMesh.material.color;
        }
    }
    [SerializeField] protected Transform shieldHolderTF;
    [Header("Weapon")] [SerializeField] protected Transform weaponHolderTF;
    [Header("Attack")] 
    [SerializeField]protected WeaponController weaponController;
    [SerializeField] private LayerMask characterLayer;
    protected Dictionary<Transform,int> targets = new Dictionary<Transform, int>();
    public Vector3 target;
    [SerializeField] private int killCount;
    private GameObject currentShield;
    private GameObject currentHat;
    public int KillCount
    {
        get => killCount;
        set => killCount = value;
    }
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
    
    // NOTE:
    //
    
    protected virtual void Awake()
    {
       
    }

    protected virtual void OnEnable()
    {
        GameManager.Instance.GameChangeStateEvent.AddListener(OnGameChangeState);
    }

    protected virtual void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameChangeStateEvent.RemoveListener(OnGameChangeState);
        }
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
        UpdateUI();
    }
    
    
    
    
    public virtual void Init()
    {
        //default
        gameObject.SetActive(true);
        SetKillCountPanelColor(skinSkinMesh.sharedMaterial.color);
        CharacterState = CharacterState.Init;
        speed = FixVariable.CHARACTER_SPEED;
        killCount = 0;
        CacheComponentManager.Instance.CCCache.Add(gameObject);
        if (weaponController != null)
        {
            weaponController.gameObject.SetActive(false);
        }
        weaponController = GetCharacterWeapon();
        CacheComponentManager.Instance.TFCache.Get(gameObject).localScale=Vector3.one;
        targets.Clear();
        SetCharacterNameUI(GameManager.Instance.DataController.GetPlayerName(), skinSkinMesh.material.color);
    }
    private void OnGameChangeState(GameState oldState, GameState newState)
    {
        if (newState == GameState.PlayState)
        {
            CharacterState = CharacterState.Idle;
        }
    }
    protected virtual void OnCharacterChangeState(CharacterState oldState,CharacterState newState)
    {
        ChangeAnimation(newState.ToString());
        
        // if character isnt moving, not allow update rotation
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
        
        switch (newState)
        {
            case CharacterState.Attack:
                RotateToTarget(target);
                break;
            case CharacterState.Die:
                OnCharacterDie();
                break;
        }
    }
    public void SetCharacterPossision(Vector3 nPosision)
    {
        this.NavMesh.Warp(nPosision);
        SetCharacterRotation();
    }

    private void SetCharacterRotation()
    {
        CacheComponentManager.Instance.TFCache.Get(gameObject).rotation=Quaternion.identity;
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

        var selfTransform = CacheComponentManager.Instance.TFCache.Get(gameObject);
        var attackRadius = selfTransform.localScale.x * 5;
        var characterInArea = Physics.OverlapSphere(selfTransform.position, attackRadius, characterLayer,QueryTriggerInteraction.Collide);
        targets.Clear();
        for (int i = 0; i < characterInArea.Length; i++) {
            
            // Skip self
            if (characterInArea[i].gameObject==gameObject)
            {
                continue;
            }
            //
            var targetController = CacheComponentManager.Instance.CCCache.Get(characterInArea[i].gameObject);
            if (targetController.IsAlive())
            {
                targets.Add(CacheComponentManager.Instance.TFCache.Get(characterInArea[i].gameObject),0);
            }
        }
        
        if (targets.Count > 0)
        {
            return true;
        }

        return false;
    }
    
    // NOTE;
    // Find the nearest target in area
    //
    protected Vector3 FindNearestTarget()
    {
        float minDIs = 1000;
        Vector3 tPosision = Vector3.zero;
        for (int i = 0; i < targets.Count; i++)
        {
            var targetKeyValue = targets.ElementAt(i);
            var distanceToTarget =
                (CacheComponentManager.Instance.TFCache
                    .Get(gameObject).position - targetKeyValue.Key.position).magnitude;
            if (distanceToTarget < minDIs)
            {
                tPosision = targetKeyValue.Key.position;
                minDIs = distanceToTarget;
            }
        }
        return tPosision;
    }
    // NOTE:
    // IsCharacterAlive
    //
    public bool IsAlive()
    {
        return gameObject.activeSelf && CharacterState != CharacterState.Die;
    }
    
    protected abstract void Move();
    
    //NOTE: be called by animation Event (attack clip)
    public  void Attack()
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

    protected virtual void OnCharacterDie()
    {
        GameManager.Instance.OnCharacterDie(this);
        StartCoroutine(DelaySetActive());
    }

    private IEnumerator DelaySetActive()
    {
        yield return new WaitForSeconds(FixVariable.TIME_DISABLE_AFTER_CHARACTER_DIE);
        if (CharacterState == CharacterState.Die)
        {
            gameObject.SetActive(false);
        }
    }
    

    protected virtual void DeSpawn()
    {
       
    }
    protected virtual void ChangeAnimation(string newParam)
    {
        animator.ResetTrigger(currentParam);
        currentParam = newParam;
        animator.SetTrigger(newParam);
    }
    protected void SetCharacterNameUI(string name, Color color)
    {
        characterNameText.text = name;
        killImage.color = color;
    }
    protected void UpdateKillCountUI(int killCount)
    {
        killCountText.text = killCount.ToString();
    }

    protected void SetKillCountPanelColor(Color color)
    {
        killImage.color = color;
    }
    
    protected virtual void UpdateUI()
    {
        if (CharacterState!=CharacterState.Init)
        {
            if (!uiTransform.gameObject.activeSelf)
            {
                uiTransform.gameObject.SetActive(true);
            }
            var mainCam = CameraController.Instance.MainCam;
            uiPanelTransform.position=mainCam.WorldToScreenPoint(uiTransform.position);
        }
        else
        {
            if (uiTransform.gameObject.activeSelf)
            {
                uiTransform.gameObject.SetActive(false);
            }
        }
        UpdateKillCountUI(KillCount);
    }

    public virtual void OnCharacterKillEnemy()
    {
        KillCount++;
        if (KillCount % FixVariable.CHARACTER_KILL_TO_LEVELUP == 0
            &&FixVariable.MAX_CHARACTER_LEVEL*FixVariable.CHARACTER_KILL_TO_LEVELUP>=killCount)
        {
            OnCharacterLevelUp();
        }
    }
    protected virtual void OnCharacterLevelUp()
    {
        CacheComponentManager.Instance.TFCache.Get(gameObject).localScale *= 1.1f;
    }

    protected void SetHat(GameObject hatPrefab)
    {
        if (currentHat != null)
        {
            Destroy(currentHat);
        }
        if (hatPrefab != null)
        {
            
            currentHat=Instantiate(hatPrefab);
            currentHat.GetComponent<CharacterObjectController>().Init(hatHolderTF);
            //
        }
    }
    protected void SetPant(Material pant)
    {
        if (pant != null)
        {
            pantSkinMesh.material = pant;
        }
    }
    protected void SetShield(GameObject shieldPrefab)
    {
        if (currentShield != null)
        {
            Destroy(currentShield);
        }
        if (shieldPrefab != null)
        {
            currentShield=Instantiate(shieldPrefab);
            currentShield.GetComponent<CharacterObjectController>().Init(shieldHolderTF);
        }
    }
    protected void SetSkin(Material skin)
    {
        if (skin != null)
        {
            skinSkinMesh.material = skin;
        }
    }
    
}
