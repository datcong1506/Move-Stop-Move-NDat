using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RevivalUIController : UICanvas
{
    [SerializeField] private int _t;
    [SerializeField] private TextMeshProUGUI _countDownTMP;
    [SerializeField] private Transform countDownEffectTF;
    
    private void OnEnable() {
        Init();    
    }
    
    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        Destroy(gameObject);
    }
    
    
    private void Init(){
        _countDownTMP.text=_t.ToString();
        StartCoroutine(CountDown());
    }
    public void ExitButton(){
        ChoseLose();
    }
    public void FreeButton(){
        ChoseRivival();
    }
    private void ChoseLose(){
        GameManager.Instance.Lose();
    }
    private void ChoseRivival()
    {
        GameManager.Instance.Rivial();
    }
    private void Update()
    {
        countDownEffectTF.Rotate(Vector3.forward*-1*360*Time.deltaTime);
    }

    IEnumerator CountDown()
    {
        var t = _t;
        for (int i = 0; i < t; i++)
        {
            yield return new WaitForSeconds(1);
            _t-=1;
            _countDownTMP.text=_t.ToString();
            if(_t<=0){
                ChoseLose();
            }
        }
    }

    
}
