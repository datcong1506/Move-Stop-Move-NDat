using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FixVariable
{
    public const int MAX_CHARACTER_LEVEL = 3;
    public const int CHARACTER_KILL_TO_LEVELUP = 6;
    
    public const float CHARACTER_SPEED = 5f;
    public const float TIME_DISABLE_AFTER_CHARACTER_DIE = 1f;
    public const float AI_MINDISTANCE_TOPLAYER = 5f;

//
    public const float CONDICATE_PERCENT = 5f;
    
    //string
    public const string DATA_PATH = "/GameData.json";

    //character anim param
    public const string IDLE_PARAM="Idle";
    public const string RUN_PARAM="Move";
    public const string DANCE_PARAM="Dance";
    public const string DIE_PARAM="Die";
    public const string ATTACK_PARAM="Attack";
    
    
    //pauseUI anim param
    public const string VIB = "Vib";
    public const string SOUND = "Sound";
    public const string OPENSETTING = "OpenSetting";
    
    
    //
    public const string DEFAULT_SKINNAME = "NotEquipped";
}
