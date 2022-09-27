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
    [SerializeField]private bool isInShop;
    public bool IsInSHop
    {
        get
        {
            return isInShop;
        }
        set
        {
            isInShop = value;
        }
    }

    [SerializeField] private PlayerController playerController;
    [SerializeField] private List<AIController> aiOnField;

    [SerializeField]private Level level;
    [SerializeField]private int aiCount;
    public int AICount
    {
        get => aiCount;
        set => aiCount = value;
    }
    public int CharacterCount
    {
        get
        {
            return aiOnField.Count + 1;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        Application.targetFrameRate = 60;
        playerController = Instantiate(DataController.GetPlayerPrefab()).GetComponent<PlayerController>();
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
        PollingManager.Instance.DisableAllBullet();
        PollingManager.Instance.DisableAllEnemy();
        playerController.Init();
        playerController.SetCharacterPossision(Helper.GetposisionOnNavmesh(level.PlayerSpawnPosision));
        if (aiOnField != null)
        {
            for (int i = 0; i < aiOnField.Count; i++)
            {
                aiOnField[i].gameObject.SetActive(false);
            }
        }
        AICount = level.AiCount;
        aiOnField.Clear();
        SpawnAI();
        UIManager.Instance.LoadUI(UI.StartUI);
        
    }
    
    public void Play()
    {
        GameState = GameState.PlayState;
        UIManager.Instance.LoadUI(UI.PauseUI);
    }
    
    public void OnCharacterDie(CharacterController characterController)
    {
        if (characterController as PlayerController != null)
        {
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
            CacheComponentManager.Instance.CCCache.TryGet(newAI,out var aiCOntroller);
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
        playerController.CharacterState = CharacterState.Dance;
        UIManager.Instance.LoadUI(UI.WinUI);
    }

    public void Lose()
    {
        UIManager.Instance.LoadUI(UI.LoseUI);
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
            LevelManager.Instance.LoadLevel(level.SceneName);
        }
        
    }
    
#if UNITY_EDITOR
    [ContextMenu("ClearNavmeshData")]
    public void ClearNavMeshData()
    {
        NavMesh.RemoveAllNavMeshData();
    }
#endif
  
}
