using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinUiController : UICanvas
{
    [SerializeField] private Transform effect;
    [Header("Rotate Effect Speed (Degree fer sec)")]
    [SerializeField] private float rotateSpeed = 40f;

    [SerializeField] private GameObject tripleRewardPanel;
    [SerializeField] private GameObject rewardButton;
    [SerializeField] private TextMeshProUGUI goldBonusText;
    [SerializeField] private TextMeshProUGUI goldBonusTripleRewardText;
    private void Update()
    {
        effect.Rotate(Vector3.forward*-1*Time.deltaTime*rotateSpeed);
    }

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
        goldBonusText.text = GameManager.Instance.Level.GoldLevelBonus.ToString();
        goldBonusTripleRewardText.text = (GameManager.Instance.Level.GoldLevelBonus * 3).ToString();
    }
    
    public void PlayNextZoneButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        GameManager.Instance.NextLevel();
    }

    public void TrippleRewardButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        GameAudioManager.Instance.PlayClip(AudioType.Congra);
        rewardButton.SetActive(false);
        tripleRewardPanel.SetActive(true);
        GameManager.Instance.TripleReward();
    }
    
    public void ExitTripleRewardButton()
    {
        GameAudioManager.Instance.PlayClip(AudioType.Click);
        tripleRewardPanel.SetActive(false);
    }
}
