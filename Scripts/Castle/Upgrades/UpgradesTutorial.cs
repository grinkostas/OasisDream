using System;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.Scripts.Popups;
using JetBrains.Annotations;
using NepixSignals;
using StaserSDK.Stack;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class UpgradesTutorial : MonoBehaviour, ITutorialEvent
{
    [SerializeField] private View _forgeView;
    [SerializeField] private string _tutorialPassId = "ForgeTutorialStep";
    [SerializeField] private View _pointerView;
    [SerializeField] private List<UpgradeView> _upgradeViews;
    [SerializeField] private int _upgradeCost;
    [SerializeField] private float _hideForgeDelay;

    [Inject, UsedImplicitly] public Player Player { get; }
    [Inject, UsedImplicitly] public PopupFactory PopupFactory { get; }
    
    private bool Passed => ES3.Load(_tutorialPassId, false);
    public float Progress => Passed ? 1 : 0;
    public float FinalValue => 1;

    public UnityAction Finished { get; set; }
    public UnityAction Available { get; set; }
    public UnityAction<float> ProgressChanged { get; set; }
    
    public TheSignal Earned { get; } = new();
    
    public bool IsFinished() => Passed;
    public bool IsAvailable() => Passed == false;

    private void OnEnable()
    {
        if (Passed)
        {
            Finish();
            return;
        }

        if(Player.Equipment.WeaponController.Weapons.Has(x=>x.DamageUpgrade.CurrentLevel > 0))
        {
            Finish();
            return;
        }

        GiveGold();
        Init();
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
    }

    private void Init()
    {
        Available?.Invoke();
        _pointerView.Show();
        foreach (var upgradeView in _upgradeViews)
            upgradeView.Upgraded += OnUpgraded;
    }
    
    private void GiveGold()
    {
        var playerBalance = Player.Stack.SoftCurrencyStack.ItemsCount;
        if (playerBalance < _upgradeCost)
        {
            Player.Stack.SoftCurrencyStack.TryAddRange(ItemType.Diamond, _upgradeCost - playerBalance);
            Earned.Dispatch();
        }
    }

    private void OnUpgraded()
    {
        
        foreach (var upgradeView in _upgradeViews)
            upgradeView.Upgraded -= OnUpgraded;

        Finish();
    }

    private void Finish()
    {
        var previousPassed = Passed;
        _pointerView.Hide();
        ES3.Save(_tutorialPassId, true);
        Finished?.Invoke();
        ProgressChanged?.Invoke(1);
        if (previousPassed == false)
            DOVirtual.DelayedCall(_hideForgeDelay, _forgeView.Hide);
    }
    
}
