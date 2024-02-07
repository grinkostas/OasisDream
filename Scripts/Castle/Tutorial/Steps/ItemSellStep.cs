using System;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;


public class ItemSellStep : TutorialStep
{
    [SerializeField] private TutorialStepBase _previousStep;
    [SerializeField] private float _returnDelay;
    [Inject, UsedImplicitly] public Player Player { get; }
    
    protected override void OnEnter()
    {
        Player.Stack.MainStack.CountChanged += OnCountChanged;
    }
    
    protected override void OnExit()
    {
        Player.Stack.MainStack.CountChanged -= OnCountChanged;
        DOTween.Kill(_returnDelay);
    }

    private void OnCountChanged(int count)
    {
        if(Player.Stack.MainStack.ItemsCount > 0)
            return;
        DOTween.Kill(_returnDelay);
        DOVirtual.DelayedCall(_returnDelay, () =>
        {
            Exit(true);
            _previousStep.ForceEnter();
        }).SetId(_returnDelay);
    }
}
