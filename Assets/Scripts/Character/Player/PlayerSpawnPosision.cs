using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPosision : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        LevelManager.Instance.SetPlayerPosision(transform.position);
    }
}
