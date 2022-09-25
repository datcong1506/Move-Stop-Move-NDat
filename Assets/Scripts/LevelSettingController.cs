using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettingController : MonoBehaviour
{
    [SerializeField] private int maxAiCount;
    [SerializeField] private int aiCount;
    public int AICount => aiCount;
    [SerializeField] private int maxAiPerWave;
    [SerializeField] private float spawnRadius=20f;
    [SerializeField] private List<GameObject> AIsOnField;
    private Vector3 playerPosision;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        AIsOnField = new List<GameObject>();
        aiCount = maxAiCount;
        playerPosision = LevelManager.Instance.GetPlayerPosision();
        var startUI = UIManager.Instance.GetUICanvas(UI.StartUI) as StartUIController;
        LevelManager.Instance.LevelSetting = this;
        
        
        // UNDONE
        SpawnAis();
    }

    private void OnDestroy()
    {
        OnDeSpawn();
    }

    private void OnDeSpawn()
    {
        for (int i = 0; i < AIsOnField.Count; i++)
        {
            if (AIsOnField[i] != null)
            {
                AIsOnField[i].SetActive(false);
            }
        }
    }

    private void SpawnAis()
    {
        for (int i = 0; i < 10; i++)
        {
            var newAI = SpawnAi();
            AIsOnField.Add(newAI);
        }
    }

    private void OnAIDespawn(GameObject ai)
    {
        AIsOnField.Remove(ai);
        aiCount--;
        
    }
    
    private GameObject SpawnAi()
    {
        var ai = PollingManager.Instance.GetEnemy();
        CacheComponentManager.Instance.CCCache.Get(ai).NavMesh.Warp(RandomNavmeshLocation(spawnRadius, playerPosision));
        return ai;
    }

    private Vector3 RandomNavmeshLocation(float radius,Vector3 playerposision)
    {
        UnityEngine.AI.NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        for (int i = 0; i < 100; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
            if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
                if((finalPosition-playerposision).magnitude>FixVariable.AI_MINDISTANCE_TOPLAYER){
                    break;
                }
            }
        }

        return finalPosition;
    }
}
