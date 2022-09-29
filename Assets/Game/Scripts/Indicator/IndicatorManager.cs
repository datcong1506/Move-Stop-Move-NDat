using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : Singleton<IndicatorManager>
{
    [SerializeField] private GameObject indicatorUIPrefab;

    private Dictionary<CharacterController, IndicatorUIController> indicatorUIControllers;
    private ObjectPolling indicatorUIPoll;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        indicatorUIControllers = new Dictionary<CharacterController, IndicatorUIController>();
        indicatorUIPoll = new ObjectPolling(gameObject, indicatorUIPrefab, 30);
    }
    
    public void OnInScreen(CharacterController from)
    {
        if (indicatorUIControllers.ContainsKey(from))
        {
            indicatorUIControllers[from].gameObject.SetActive(false);
            indicatorUIControllers.Remove(from);
        }
    }

    public void OnOutScreen(CharacterController from)
    {
        if (!indicatorUIControllers.ContainsKey(from))
        {
            var newIndicatorUIController = indicatorUIPoll.Instantiate()
                .GetComponent<IndicatorUIController>();
            newIndicatorUIController.Init(this,from);
            indicatorUIControllers.Add(from,newIndicatorUIController);
        }
    }
}
