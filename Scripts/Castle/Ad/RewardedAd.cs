using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NepixSignals;

public class RewardedAd
{
    private string _rewardId = "";

    public bool IsCompleted { get; private set; } = false;
    public TheSignal ErrorClaimed { get; } = new();
    public TheSignal Completed { get; } = new();
    public TheSignal Hidden { get; } = new();
    public TheSignal Ended { get; } = new();

    public RewardedAd(string rewardId)
    {
        _rewardId = rewardId;
    }
/*
    public void ShowReward()
    {
        IsCompleted = false;
            
        if (MaxSdk.IsRewardedAdReady(_rewardId) == false)
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnAdLoaded;
            return;
        }

        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent  += OnAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnAdHidden;
        MaxSdk.ShowRewardedAd(_rewardId);
        
    }

    private void OnAdLoaded(string id, MaxSdk.AdInfo adInfo)
    {
        Debug.Log($"Loaded reward {id}");
        if(id != _rewardId)
            return;
        
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnAdLoaded;
        ShowReward();
    }

    private void OnAdReceivedRewardEvent(string id, MaxSdk.Reward reward, MaxSdk.AdInfo adInfo)
    {
        if(id != _rewardId)
            return;
        
        Debug.Log($"reward {id} claimed");
        IsCompleted = true;
        Clear();
        Ended.Dispatch();
        Completed.Dispatch();
    }
    
    private void OnAdHidden(string id, MaxSdk.AdInfo adInfo)
    {
        if(id != _rewardId)
            return;
        Debug.Log($"reward {id} hidden");
        IsCompleted = true;
        Clear();
        Ended.Dispatch();
        Hidden.Dispatch();
    }
    
    private void OnAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        if(adUnitId != _rewardId)
            return;
        Debug.Log($"reward {adUnitId} error {errorInfo.Message}");
        Clear();
        Ended.Dispatch();
        ErrorClaimed.Dispatch();
    }
    
    private void OnAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        if(adUnitId != _rewardId)
            return;
        
        Debug.Log($"reward {adUnitId} error {errorInfo.Message}");
        Clear();
        Ended.Dispatch();
        ErrorClaimed.Dispatch();
    }

    private void Clear()
    {
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent  -= OnAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnAdHidden;
    }
    */
}
