using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataController:MonoBehaviour
{
    [SerializeField] private DynamicData dynamicData;

    [SerializeField] private StaticData staticData;

    public StaticData StaticData
    {
        get
        {
            return staticData;
        }
    }
    //
    private string path;
    private string persistentPath = "";
    //
    
    private void Init()
    {
        path = Application.dataPath
                   + Path.AltDirectorySeparatorChar + FixVariable.DATA;
        persistentPath = Application.persistentDataPath
                         + Path.AltDirectorySeparatorChar + FixVariable.DATA;
    }

    public void LoadData()
    {
        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        DynamicData data = JsonUtility.FromJson<DynamicData>(json);
        this.dynamicData = data;
    }

    public void WriteData()
    {
        StreamWriter writer = new StreamWriter(path);
        writer.Write(JsonUtility.ToJson(dynamicData));
    }
}
