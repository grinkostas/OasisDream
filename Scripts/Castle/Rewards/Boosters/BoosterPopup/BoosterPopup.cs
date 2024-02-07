using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NepixSignals;
using StaserSDK.Utilities;
using UnityEngine.UI;
using Zenject;

public abstract class BoosterPopup : MonoBehaviour, IPoolItem<BoosterPopup>
{
    [SerializeField] private string _rewardId;
    [SerializeField] private RewardButton _watchButton;
    [SerializeField] private View _popupHideView;
    
    [Inject, UsedImplicitly] public PauseManager PauseManager { get; }
    public IPool<BoosterPopup> Pool { get; set; }
    public bool IsTook { get; set; }

    public TheSignal<BoosterPopup> Disabled { get; } = new();

    private void OnEnable()
    {
        PauseManager.Pause(this);
        _watchButton.Pressed.On(Reward);
        _watchButton.Error.On(Hide);
    }

    private void OnDisable()
    {
        _watchButton.Pressed.Off(Reward);
        _watchButton.Error.Off(Hide);
        PauseManager.Resume(this);
        Disabled.Dispatch(this);
    }

    private void Reward()
    {
        Disabled.Dispatch(this);
        TakeReward();
    }
    

    protected abstract void TakeReward();
    
    public void TakeItem()
    {
    }

    public void ReturnItem()
    {
    }

    public void Hide()
    {
        _popupHideView.Hide();
    }
}
