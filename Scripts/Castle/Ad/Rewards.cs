using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NepixSignals;
using StaserSDK.Utilities;
using Zenject;

public class Rewards
{
    /*
    private List<string> _ids = new()
    {
        #if UNITY_ANDROID || UNITY_EDITOR
            "7eb694e3eb50aa28",
            "ca-app-pub-7247420791687728/1923313268",
            "3158579424445906_3158583651112150",
            "Rewarded_Android"
        #endif
                
        #if UNITY_IOS
            "3158579424445906_3158584694445379",
            "ca-app-pub-7247420791687728/1194407723",
            "Rewarded_iOS"
        #endif
    };

    private string _nextAdId = "";
    
    [Inject, UsedImplicitly] public PauseManager PauseManager { get; }

    public TheSignal Rewarded { get; } = new();

    [Inject]
    public void Initialize()
    {
        LoadNextReward();
    }

    private void LoadNextReward()
    {
        _nextAdId = _ids.Random();
        MaxSdk.LoadRewardedAd(_nextAdId);
    }
    
    public RewardedAd Show()
    {
        var rewardAd =  new RewardedAd(_nextAdId);
        PauseManager.Pause(this);
        rewardAd.Completed.Once(Rewarded.Dispatch);
        rewardAd.Ended.Once(() =>
        {
            PauseManager.Resume(this);
            rewardAd.Completed.Off(Rewarded.Dispatch);
        });
        rewardAd.ShowReward();
        LoadNextReward();
        return rewardAd;
    }
    */
}
