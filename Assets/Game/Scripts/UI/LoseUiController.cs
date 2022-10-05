using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseUiController : UICanvas
{
    [SerializeField] private TextMeshProUGUI rank;
    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        Destroy(gameObject);
    }

    private void Init()
    {
        rank.text = (GameManager.Instance.CharacterAliveCount+1).ToString();
    }

    public void TryAgainButton()
    {
        GameManager.Instance.TryAgain();
    }
}
