using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public enum GameState
{
    InitState,
    PlayState,
    WinState,
    LoseState
}
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private DataController dataController;
    public DataController DataController
    {
        get
        {
            return dataController;
        }
    }
    public UnityEvent<GameState, GameState> GameChangeStateEvent;
    private GameState gameState;
    public GameState GameState
    {
        get
        {
            return gameState;
        }
        set
        {
            var oldState = gameState;
            gameState = value;  
            GameChangeStateEvent?.Invoke(oldState,value);
        }
    }

    private int rank;
    public int Rank
    {
        get { return rank; }
    }
        [SerializeField] private PlayerController playerController;
    public PlayerController PlayerController
    {
        get
        {
            return playerController;
        }
    }
    [SerializeField] private List<AIController> aiOnField;
    [SerializeField] private CharacterPreviewController characterPreviewController;
    public CharacterPreviewController CharacterPreviewController => characterPreviewController;
    
    [SerializeField]private Level level;
    public Level Level => level;

    [SerializeField]private int aiCount;
    public int AICount
    {
        get => aiCount;
        set => aiCount = value;
    }
    public int CharacterAliveCount
    {
        get
        {
            return aiCount + (playerController.CharacterState!=CharacterState.Die?1:0);
        }
    }
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    private void Init()
    {
        SpawnPlayer();
        level = DataController.GetCurrentLevel();
        LevelManager.Instance.LoadLevel(level.SceneName);
        SceneManager.sceneLoaded += OnLoadScene;
        aiOnField = new List<AIController>();
    }
    
    private void OnLoadScene(Scene arg0, LoadSceneMode arg1)
    {
        if (arg1 == LoadSceneMode.Additive)
        {
            StartLevel();
        }
    }

    private void StartLevel()
    {
        GameState = GameState.InitState;
        PollingManager.Instance.DisableAllBullet();
        PollingManager.Instance.DisableAllEnemy();
        playerController.Init();
        playerController.SetCharacterPossision(Helper.GetposisionOnNavmesh(level.PlayerSpawnPosision));
        AICount = level.AiCount;
        aiOnField.Clear();
        SpawnAI();
        UIManager.Instance.LoadUI(UI.StartUI);
        CameraController.Instance.Init();
    }
    
    public void Play()
    {
        GameState = GameState.PlayState;
        UIManager.Instance.LoadUI(UI.PauseUI);
        CameraController.Instance.Play();
    }
    
    public void OnCharacterDie(CharacterController characterController)
    {
        if (characterController as PlayerController != null)
        {
            rank = CharacterAliveCount;
            UIManager.Instance.LoadUI(UI.RevivalUI);
        }
        else
        {
            aiOnField.Remove(characterController as AIController);
            AICount--;
            if (AICount <= 0)
            {
                StartCoroutine(DelayCheckWinOrLose());
            }
            SpawnAI();
        }
    }
    
    public void Rivial()
    {
        playerController.Init();
        playerController.CharacterState = CharacterState.Idle;
        UIManager.Instance.LoadUI(UI.PauseUI);
    }

    private void SpawnAI()
    {
        while (aiOnField.Count<level.AiPerWave)
        {
            if (AICount < level.AiPerWave)
            {
                break;
            }
            var newAI = PollingManager.Instance.GetEnemy();
            var aiCOntroller= CacheComponentManager.Instance.CCCache.Get(newAI);
            aiOnField.Add(aiCOntroller as AIController);
            aiCOntroller.Init();
            if (GameState == GameState.PlayState)
            {
                aiCOntroller.CharacterState = CharacterState.Idle;
            }
            aiCOntroller.SetCharacterPossision(
                Helper.RandomNavmeshLocation(level.RadiusToCenter,level.Center,
                    CacheComponentManager.Instance.TFCache.Get(playerController.gameObject).position));
        }
    }
    // note : all ai die, player not die and weapon still fly
    private IEnumerator DelayCheckWinOrLose()
    {
        while (PollingManager.Instance.GetBulletCount()!=0)
        {
            yield return null;
        }

        if (playerController.CharacterState == CharacterState.Die)
        {
            // lose
            Lose();
        }
        else
        {
            // win
            Win();
        }
    }

    private void Win()
    {
        GameState = GameState.WinState;
        DataController.GoldCount += level.GoldLevelBonus;
        playerController.CharacterState = CharacterState.Dance;
        UIManager.Instance.LoadUI(UI.WinUI);
        GameAudioManager.Instance.PlayClip(AudioType.Win);
    }

    public void TripleReward()
    {
        DataController.GoldCount += level.GoldLevelBonus*2;
    }

    public void Lose()
    {
        UIManager.Instance.LoadUI(UI.LoseUI);
        GameAudioManager.Instance.PlayClip(AudioType.Lose);
        //set rank
        if (DataController.DynamicData.BestRank > rank)
        {
            DataController.DynamicData.BestRank = rank;
        }
    }

    public void TryAgain()
    {
        LevelManager.Instance.UnLoadLevel(level.SceneName);
        LevelManager.Instance.LoadLevel(level.SceneName);
    }

    public void NextLevel()
    {
        if (level.HaveNextLevel)
        {
            LevelManager.Instance.UnLoadLevel(level.SceneName);
            level = level.NextLevel;
            DataController.SetLevel(level.SceneName);
            DataController.DynamicData.BestRank = 99999;
            LevelManager.Instance.LoadLevel(level.SceneName);
        }
        else
        {
            TryAgain();
        }
        
    }

    
    private void SpawnPlayer()
    {
        if (playerController == null)
        {
            playerController = Instantiate(DataController.GetPlayerPrefab()).GetComponent<PlayerController>();
        }
    }

    private void DisablePlayer()
    {
        if (playerController != null)
        {
            playerController.gameObject.SetActive(false);
            
        }
    }

    public void OnPlayerKillEnemy()
    {
        DataController.GoldCount += level.GoldKillBonus;
    }
    

    private void EnablePlayer()
    {
        if (!playerController.gameObject.activeSelf)
        {
            playerController.gameObject.SetActive(true);
            playerController.Init();
        }
    }

    private void SpawnCharacterPreview()
    {
        if (characterPreviewController == null)
        {
            characterPreviewController = Instantiate(DataController.GetCharacterPreviewPrefab())
                .GetComponent<CharacterPreviewController>();
            characterPreviewController.Init(CacheComponentManager.Instance
                .TFCache.Get(playerController.gameObject).position);
        }
        else
        {
            characterPreviewController.gameObject.SetActive(true);
            characterPreviewController.Init(CacheComponentManager.Instance
                .TFCache.Get(playerController.gameObject).position);
        }
    }
    
    private void DestroyCharacterPreview()
    {
        if (characterPreviewController != null)
        {
            characterPreviewController.gameObject.SetActive(false);
        }
    }

    public void EnterSkinShop()
    {
        DisablePlayer();
        SpawnCharacterPreview();
        CameraController.Instance.EnterSkinShop();
    }

    public void ExitSkinShop()
    {
        EnablePlayer();
        DestroyCharacterPreview();
        CameraController.Instance.ExitSkinShop();
    }

    public void EnterWeaponShop()
    {
        DisablePlayer();
    }

    public void ExitWeaponShop()
    {
        EnablePlayer();
    }
    
    
#if UNITY_EDITOR
    [ContextMenu("ClearNavmeshData")]
    public void ClearNavMeshData()
    {
        NavMesh.RemoveAllNavMeshData();
    }
#endif
  
}
