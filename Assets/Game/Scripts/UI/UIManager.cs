using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<UI, UICanvas> storageUIs = new Dictionary<UI, UICanvas>();
    private UICanvas previousCanvas;
    [SerializeField] private Transform selfTransform;
    
    protected override void Awake()
    {
        base.Awake();
    }

    public GameObject InstanceUI(UI uIId)
    {
        GameObject uiPrefab = GetUIPrefab(uIId);
        GameObject rs = null;
        if (uiPrefab != null)
        {
            rs = Instantiate(uiPrefab, selfTransform);
        }
        return rs;
    }
    public GameObject GetUIPrefab(UI uIID)
    {
        return Resources.Load("UI/" + uIID.ToString()) as GameObject;
    }

    public UICanvas GetUICanvas(UI uIID)
    {
        if (storageUIs.ContainsKey(uIID))
        {
            return storageUIs[uIID];
        }

        return null;
    }
    public void LoadUI(UI uIId)
    {
        bool exsistUI = storageUIs.ContainsKey(uIId);
        if (exsistUI)
        {
            if (storageUIs[uIId] == null)
            {
                var newUICanvasGO = InstanceUI(uIId);
                storageUIs[uIId] = newUICanvasGO.GetComponent<UICanvas>();
            }
        }
        else
        {
            var newUICanvasGO = InstanceUI(uIId);
            storageUIs.Add(uIId, newUICanvasGO.GetComponent<UICanvas>());
        }
        var uiCanvas = storageUIs[uIId];
        if (previousCanvas != null)
        {
            previousCanvas.OnExit();
        }
        uiCanvas.OnEnter();
        previousCanvas = uiCanvas;

    }
}
