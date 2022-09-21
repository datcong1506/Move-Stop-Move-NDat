using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T singleton;

    public static T Instance
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<T>();
            }
            return singleton;
        }
    }
    protected virtual void Awake()
    {
        if (singleton == null)
        {
            singleton = (T)this;
        }
        else
        {
            if (singleton != this)
            {
                Destroy(gameObject);
            }
        }
    }
}

public class DataSingleton<T> : ScriptableObject where T : DataSingleton<T>
{
    private static T singleton;
    public static T Instance
    {
        get
        {
            if (singleton == null)
            {
                T[] assets = Resources.LoadAll<T>("");
                if (assets == null || assets.Length < 1)
                {
                    throw new System.Exception("countnt find any singleton scriableobject in resources");
                }
                else
                {
                    if (assets.Length > 1)
                    {
                        Debug.LogWarning("multiple singleton of a scriableobject found in resource");
                    }
                }
                singleton = assets[0];
            }
            return singleton;
        }
    }
}
