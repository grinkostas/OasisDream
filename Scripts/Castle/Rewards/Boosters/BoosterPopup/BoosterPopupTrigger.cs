using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Zenject;

public class BoosterPopupTrigger : MonoBehaviour
{
    [SerializeField] private int _attemptsToFirstTrigger;
    [SerializeField] private int _attemptsToNextTriggers;
    [SerializeField] private BoosterPopup _boosterPopupPrefab;
    [SerializeField] private float _returnDelay = 1.0f;

    [Inject, UsedImplicitly] public Player Player { get; }
    [Inject, UsedImplicitly] public OverlayCanvas OverlayCanvas { get; }
    [Inject, UsedImplicitly] public DiContainer DiContainer { get; }
    
    private int _triggerCount = 0;
    private int _attempts = 0;
    
    private SimplePool<BoosterPopup> _popup;

    private SimplePool<BoosterPopup> Popup
    {
        get
        {
            InitPool();
            return _popup;
        }
    }

    private void InitPool()
    {
        if (_popup != null)
            return;
        _popup = new SimplePool<BoosterPopup>(_boosterPopupPrefab, 1, OverlayCanvas.Canvas.transform);
        _popup.Initialize(DiContainer);
    }

    public void SpawnPopup()
    {
        Popup.Get().Disabled.Once(OnPopupDisable);
    }

    protected void ApplyAttempt()
    {
        _attempts++;
    }
    
    protected void TryTrigger()
    {
        if(_triggerCount == 0 && _attempts >= _attemptsToFirstTrigger)
            Trigger();
        if(_triggerCount > 0 && _attempts >= _attemptsToNextTriggers)
            Trigger();
    }

    private void Trigger()
    {
        _triggerCount++;
        _attempts = 0;
        SpawnPopup();
    }

    private void OnPopupDisable(BoosterPopup boosterPopup)
    {
        boosterPopup.Hide();
        DOVirtual.DelayedCall(_returnDelay, ()=> boosterPopup.Pool.Return(boosterPopup));
    }
}
