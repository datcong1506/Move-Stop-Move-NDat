using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


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

    private bool isInShop;
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
    
    
    private void Init()
    {
        Application.targetFrameRate = 60;
    }

    public void Play()
    {
        GameState = GameState.PlayState;
    }
}
