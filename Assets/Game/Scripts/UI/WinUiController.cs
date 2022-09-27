using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUiController : UICanvas
{
    [SerializeField] private Transform effect;
    [Header("Rotate Effect Speed (Degree fer sec)")]
    [SerializeField] private float rotateSpeed = 40f;

    [SerializeField] private GameObject tripleRewardPanel;
    [SerializeField] private GameObject rewardButton;
    private void Update()
    {
        effect.Rotate(Vector3.forward*-1*Time.deltaTime*rotateSpeed);
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
        Destroy(gameObject);
    }

    
    public void PlayNextZoneButton()
    {
        GameManager.Instance.NextLevel();
    }

    public void TrippleRewardButton()
    {
        rewardButton.SetActive(false);
        tripleRewardPanel.SetActive(true);
    }

    public void ExitTripleRewardButton()
    {
        tripleRewardPanel.SetActive(false);
    }
}
