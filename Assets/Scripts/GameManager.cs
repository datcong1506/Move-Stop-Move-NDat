using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void Play()
    {

    }

    public void Pause()
    {

    }

    public void NextLevel()
    {

    }

    public void TryAgain()
    {

    }
}
