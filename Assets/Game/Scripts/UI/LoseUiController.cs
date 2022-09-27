using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseUiController : UICanvas
{
    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        Destroy(gameObject);
    }

    public void TryAgainButton()
    {
        GameManager.Instance.TryAgain();
    }
}
