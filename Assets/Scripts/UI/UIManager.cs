using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<UI, UICanvas> storageUIs = new Dictionary<UI, UICanvas>();
    private UICanvas previousCanvas;
    [SerializeField] private UI startUI;
    [SerializeField] private Transform selfTransform;
    
    protected override void Awake()
    {
        base.Awake();
        LoadUI(startUI);
    }

    public GameObject InstanceUI(UI uIId)
    {
        GameObject uiPrefab = GameManager.Instance.DataController.StaticData.GetUIPrefab(uIId);
        GameObject rs = null;
        if (uiPrefab != null)
        {
            rs = Instantiate(uiPrefab, selfTransform);
        }
        return rs;
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
