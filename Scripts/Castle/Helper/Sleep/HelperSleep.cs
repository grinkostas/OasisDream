using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NepixSignals;
using StaserSDK;
using StaserSDK.Interactable;

public class HelperSleep : MonoBehaviour
{
    [SerializeField] private WaitHelperState _waitHelperState;
    [SerializeField] private ZoneBase _wakeUpZone;
    [SerializeField] private float _wakeUpDelay;
    [SerializeField] private float _speedBoosterMultiplayer = 2.0f;
    [SerializeField] private float _speedBoostDuration;

    public Helper Helper => _waitHelperState.Helper;
    public TheSignal WokeUp { get; } = new();
    
    private void OnEnable()
    {
        _wakeUpZone.OnInteract += OnInteract;
    }
    
    private void OnDisable()
    {
        _wakeUpZone.OnInteract -= OnInteract;
    }

    private void OnInteract(InteractableCharacter character)
    {
        if(_waitHelperState.enabled && _waitHelperState.CanExit)
            WakeUp();
    }
    
    private void WakeUp()
    {
        WokeUp.Dispatch();
        ApplySpeedModifier();
        _waitHelperState.KillTween();
        DOVirtual.DelayedCall(_wakeUpDelay, _waitHelperState.Exit);
    }

    private void ApplySpeedModifier()
    {
        _waitHelperState.Helper.Handler.AddSpeedModifier(new SpeedModifier(this, new FloatModifier(_speedBoosterMultiplayer, NumericAction.Multiply)));
        DOVirtual.DelayedCall(_speedBoostDuration, () => _waitHelperState.Helper.Handler.RemoveSpeedModifier(this));
    }
}
