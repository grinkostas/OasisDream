using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NepixSignals;
using StaserSDK.Utilities;
using Zenject;

public class Interstitials
{
    /*
    public enum Status
    {
        Waiting,
        Loading,
        Showing
    }
    
    [Inject, UsedImplicitly] public PauseManager PauseManager { get; }
    
    private List<string> _ids = new()
    {
        #if UNITY_ANDROID || UNITY_EDITOR
            "7a0927eca24b6faf",
            "ca-app-pub-7247420791687728/7083290544",
            "3158579424445906_3158583284445520",
            "Interstitial_Android"
        #endif
        
        #if UNITY_IOS
            "3158579424445906_3158584654445383",
            "ca-app-pub-7247420791687728/3733791217",
            "Interstitial_iOS"
        #endif
    };

    private Status _status = Status.Waiting;

    private string _nextInterstitial = "";
    public Status CurrentStatus => _status;
    public TheSignal Hidden { get; } = new();
    

    [Inject]
    public void Initialize()
    {
        LoadNextInterstitial();
    }

    private void LoadNextInterstitial()
    {
        _nextInterstitial = _ids.Random();
        Debug.Log($"Start load {_nextInterstitial} inter");
        MaxSdk.LoadInterstitial(_nextInterstitial);
    }

    public void Show()
    {
        if(_status is Status.Loading or Status.Showing)
            return;
        if (MaxSdk.IsInterstitialReady(_nextInterstitial))
        {
            ShowInterstitial();
            return;
        }
        Debug.Log($"Start Loading {_nextInterstitial} Inter");
        _status = Status.Loading;
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnLoaded;
    }

    private void OnLoaded(string interstitialID, MaxSdk.AdInfo adInfo)
    {
        Debug.Log($"Interstitial {interstitialID} Loaded");
        if(interstitialID != _nextInterstitial)
            return;
        
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnLoaded;
        ShowInterstitial();
        LoadNextInterstitial();
    }

    private void ShowInterstitial()
    {
        Debug.Log($"Interstitial {_nextInterstitial} Show");
        _status = Status.Showing;
        PauseManager.Pause(this);
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnAddHide;
        MaxSdk.ShowInterstitial(_nextInterstitial);
    }

    private void OnAddHide(string id, MaxSdk.AdInfo adInfo)
    {
        Debug.Log($"Interstitial {id} Hided");
        _status = Status.Waiting;
        LoadNextInterstitial();
        PauseManager.Resume(this);
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnAddHide;
        Hidden.Dispatch();
    }

   */
}
