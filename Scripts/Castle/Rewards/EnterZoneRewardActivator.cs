using System;
using DG.Tweening;
using JetBrains.Annotations;
using NepixSignals;
using StaserSDK.Interactable;
using UnityEngine;
using Zenject;


public class EnterZoneRewardActivator : MonoBehaviour
{
    [SerializeField] private RewardBooster _rewardBooster;
    [SerializeField] private float _takeDelay;
    [SerializeField] private float _hideDelay;
    [SerializeField] private Transform _zoomTarget;
    [SerializeField] private float _zoomDuration;
    
    public TheSignal Activated { get; } = new();


    private void OnEnable()
    {
        _rewardBooster.Zone.OnInteract += OnInteract;
    }
    
    private void OnDisable()
    {
        _rewardBooster.Zone.OnInteract -= OnInteract;
    }

    private void OnInteract(InteractableCharacter interactableCharacter)
    {
        if(enabled == false)
            return;
        Activated.Dispatch();
        _rewardBooster.Zone.OnInteract -= OnInteract;
        DOTween.Kill(this);
        DOVirtual.DelayedCall(_takeDelay, () =>
        {
            _rewardBooster.Take();
            _zoomTarget.DOScale(0, _zoomDuration).SetEase(Ease.InBack).SetDelay(_hideDelay).SetId(this);
        }).SetId(this);
    }
}
