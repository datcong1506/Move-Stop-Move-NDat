using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    private GameObject player;
    private List<GameObject> aisOnField;
    private LevelSettingController levelSetting;
    public LevelSettingController LevelSetting
    {
        get => levelSetting;
        set => levelSetting = value;
    }
    private void Start()
    {
        Init();
        // NavMesh.RemoveAllNavMeshData();
    }

    private void Init()
    {
        aisOnField = new List<GameObject>();
        player = 
            Instantiate(GameManager.Instance.DataController
            .GetPlayerPrefab());
        LoadLevel(1);
    }

    public Vector3 GetPlayerPosision()
    {
        return CacheComponentManager.Instance.TFCache.Get(player).position;
    }
    public void SetPlayerPosision(Vector3 nPosision)
    {
        CacheComponentManager
            .Instance.CCCache
            .Get(player).NavMesh.Warp(GetPosisionOnNavMesh(nPosision));
    }
    private Vector3 GetPosisionOnNavMesh(Vector3 sourcePosision)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(sourcePosision, out hit, 5f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return  Vector3.zero;
    }
    public void OnCharacterDie(CharacterController characterController)
    {
        //if is player
        var playerController = characterController as PlayerController;
        if(playerController!=null)
        {
            // game lose
            Lose();
            return;
        }
        
    }
    private void LoadLevel(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Additive);
    }
    private void UnLoadLevel(int index)
    {
        SceneManager.UnloadSceneAsync(index);
    }
    public void Play()
    {
        
    }
    private void Lose()
    {
        
    }

    private void Win()
    {
        
    }
}
