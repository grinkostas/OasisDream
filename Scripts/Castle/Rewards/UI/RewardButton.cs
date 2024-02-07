using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine.UI;
using Zenject;

public class RewardButton : MonoBehaviour
{
    
    [SerializeField] private string _rewardButtonId;
    [SerializeField] private RectTransform _rewardIcon;
    [SerializeField] private Button _button;
    [SerializeField] private bool _firstFree;

    [Inject, UsedImplicitly] public Rewards Rewards { get; }
    public int RewardTakeCount => ES3.Load(_rewardButtonId, 0);
    
    public TheSignal OnClick { get; } = new();
    public TheSignal Error { get; } = new();
    public TheSignal Pressed { get; } = new();
    
    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
        if(RewardTakeCount == 0 && _firstFree)
            _rewardIcon.gameObject.SetActive(false);
        else
            _rewardIcon.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        OnClick.Dispatch();
        int rewardsCount = RewardTakeCount;
        ES3.Save(_rewardButtonId, RewardTakeCount+1);
        if (rewardsCount == 0 && _firstFree)
        {
            Pressed.Dispatch();
            return;
        }

        ShowReward();
    }

    private void ShowReward()
    {
        /*
        var rewardAd = Rewards.Show();
        rewardAd.Completed.Once(Complete);
        rewardAd.ErrorClaimed.Once(ErrorClaimed);
        */
    }
    
    private void Complete()
    {
        Pressed.Dispatch();
    }

    private void ErrorClaimed()
    {
        Error.Dispatch();
    }
}
