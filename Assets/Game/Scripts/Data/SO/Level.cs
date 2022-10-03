using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Level",menuName ="Data/Level")]
public class Level : ScriptableObject
{
    public string SceneName;
    public int AiCount;
    public float RadiusToCenter;
    public Vector3 Center;
    public int AiPerWave;
    public Level NextLevel;
    public Vector3 PlayerSpawnPosision;
    public bool HaveNextLevel
    {
        get
        {
            return NextLevel != null;
        }
    }
    public int GoldLevelBonus;
    public int GoldKillBonus;
}
