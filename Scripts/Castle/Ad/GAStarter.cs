using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using Zenject;

public class GAStarter : MonoBehaviour
{
    /*
    [Inject, UsedImplicitly] public Interstitials Interstitials { get; }
    [Inject, UsedImplicitly] public Rewards Rewards { get; }
    
    public bool Initialized = false;
    private void Awake()
    {
        GameAnalytics.Initialize();
        SDKController.Initialized.Once(() => Initialized = true);
        SDKController.Initialize();
    }

    [Button()]
    private void ShowInter()
    {
        Interstitials.Show();
    }

    [Button()]
    private void ShowReward()
    {
        Rewards.Show();
    }
    */
}
