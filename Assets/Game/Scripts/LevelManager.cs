using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
  
    private void Start()
    {
        Init();
    }

    private void Init()
    {
    }
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name,LoadSceneMode.Additive);
    }
    public void UnLoadLevel(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }
}
