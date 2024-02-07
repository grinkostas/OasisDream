using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using NepixSignals;
using Zenject;

public class InterstitialBehaviour : MonoBehaviour
{
    /*
    [SerializeField] private float _interstitialDelay;

    private bool _enabled => _blockers.Count == 0;
    private float _waitTime;

    private List<object> _blockers = new();

    public float TimeLeft => _waitTime;
    public float WaitTime => _interstitialDelay;

    public TheSignal TimeLeftChanged { get; } = new();

    private RevenueComponent Rewards => SDKController.Ad.Rewards;
    private RevenueComponent Interstitials => SDKController.Ad.Interstitials;
    
    private void OnEnable()
    {
        ResetTimer();
        Rewards.Started.On(OnRewardStart);
        Rewards.Ended.On(OnRewardEnd);
        Rewards.Rewarded.On(OnRewarded);
    }

    private void OnDisable()
    {
        Rewards.Started.Off(OnRewardStart);
        Rewards.Ended.Off(OnRewardEnd);
        Rewards.Rewarded.Off(OnRewarded);
    }

    private void OnRewardStart(string id)
    {
        _blockers.Add(Rewards.Started);
    }
    
    private void OnRewardEnd(string id)
    {
        _blockers.Remove(Rewards.Started);
    }
    
    private void OnRewarded(string id)
    {
        ResetTimer();
    }

    private void Update()
    {
        if (_enabled == false)
            return;
        _waitTime -= Time.deltaTime;
        TimeLeftChanged.Dispatch();
        if (_waitTime > 0)
            return;
        
        ShowInter();
    }

    private void ResetTimer()
    {
        _waitTime = _interstitialDelay;
        
        TimeLeftChanged.Dispatch();
    }

    private void ShowInter()
    {
        ResetTimer();
        _blockers.Add(Interstitials);
        var revenueAd = Interstitials.Show();
        revenueAd.Ended.Once(() => {  _blockers.Remove(Interstitials); });
    }
    */
}
