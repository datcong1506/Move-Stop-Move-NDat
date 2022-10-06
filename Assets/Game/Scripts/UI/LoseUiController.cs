using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseUiController : UICanvas
{
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private TextMeshProUGUI killByText;
    
    public override void OnEnter()
    {
        Init();
    }

    public override void OnExit()
    {
        Destroy(gameObject);
    }

    private void Init()
    {
        rank.text = (GameManager.Instance.Rank).ToString();
        killByText.text = GameManager.Instance.PlayerController.KillBy.CharacterName;
        killByText.color = GameManager.Instance.PlayerController.SkinColor;
    }

    public void TryAgainButton()
    {
        GameManager.Instance.TryAgain();
    }
}
