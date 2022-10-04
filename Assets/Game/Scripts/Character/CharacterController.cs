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
    [SerializeField] protected Transform shieldHolderTF;
    [Header("Weapon")] [SerializeField] protected Transform weaponHolderTF;
    protected WeaponType equipWeaponType;
    [Header("Attack")] 
    [SerializeField]protected WeaponController weaponController;
    protected Dictionary<Transform,int> targets = new Dictionary<Transform, int>();
    public Vector3 target;
    [SerializeField] private int killCount;
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
    private int version; // neu character nay da dc hoi sinh lai so voi lan truoc
    public int Version
    {
        get => version;
        set => version = value;
    }
    
    
    private GameObject currentShield;
    private GameObject currentHat;
    
    protected virtual void Awake()
    {
       
    }

    protected virtual void OnEnable()
    {
        GameManager.Instance.GameChangeStateEvent.AddListener(OnGameChangeState);
        Version++;
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
       
    }
    protected void LateUpdate()
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
        weaponController = GetCharacterWeapon();
        CacheComponentManager.Instance.TFCache.Get(gameObject).localScale=Vector3.one;
        targets.Clear();
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
    public bool IsAlive(int ver)
    {
        return ver==Version&&IsAlive();
    }
    public bool IsAlive()
    {
        return gameObject.activeSelf && CharacterState != CharacterState.Die;
    }
    
    //NOTE : use trigger enter to detect enemy maybe make bugs
    //  case 1: DisaaleObejct doesnt send trigger exit 
    // END
    public void AddTarget(Transform nTarget)
    {
        if(nTarget.gameObject==gameObject) return;
        if (!targets.ContainsKey(nTarget))
        {
            targets.Add(nTarget,CacheComponentManager.Instance.CCCache.Get(nTarget.gameObject).version);
        }
    }
    //NOTE : use trigger enter to detect enemy maybe make bugs
    //  case 1: DisaaleObejct doesnt send trigger exit 
    // END
    public void RemoveTarget(Transform eTarget)
    {
        if (this as PlayerController != null)
        {
            Debug.Log(gameObject.name);
        }
        
        if (targets.ContainsKey(eTarget))
        {
            targets.Remove(eTarget);
        }
    }
    protected abstract void Move();
    // be called by animation Event (attack clip)
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
        if (shieldPrefab != null)
        {
            if (currentShield != null)
            {
                Destroy(currentShield);
            }
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
