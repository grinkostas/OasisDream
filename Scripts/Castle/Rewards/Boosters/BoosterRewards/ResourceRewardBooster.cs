using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.Scripts.Stack;
using JetBrains.Annotations;
using NaughtyAttributes;
using NepixSignals;
using StaserSDK.Interactable;
using StaserSDK.Stack;
using Zenject;

public class ResourceRewardBooster : RewardBooster
{
    [SerializeField] private ItemType _rewardType;
    [SerializeField] private int _rewardCount;
    [SerializeField] private ResourceView _resourceView;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private bool _selfUsage;
    [SerializeField, ShowIf(nameof(_selfUsage))] private BoosterModel _boosterModel;
    [SerializeField] private int _maxCoins = 10;

    [Inject, UsedImplicitly] public ResourceController ResourceController { get; }
    [Inject, UsedImplicitly] public ResourceCanvasFx ResourceCanvasFx { get; }
    
    private void OnEnable()
    {
        Actualize();
    }

    public void SetReward(ItemType rewardType, int rewardCount)
    {
        _rewardType = rewardType;
        _rewardCount = rewardCount;
        Actualize();
    }

    private void Actualize()
    {
        if(_selfUsage)
            _boosterModel.Init(this);
        _resourceView.Init(_rewardType, _rewardCount);
    }

    protected override void OnTake()
    {
        int currentRewardCount = _rewardCount;
        int rewardAmount = 1;
        int coinsCount = _rewardCount;

        Zone.gameObject.SetActive(false);
        
        if (_rewardType == ItemType.Diamond)
        {
            rewardAmount = Mathf.CeilToInt(_rewardCount / (float)_maxCoins);
            coinsCount = Math.Min(_maxCoins, currentRewardCount);
        }
        
        for (int i = 0; i < coinsCount; i++)
        {
            var amount = Mathf.Min(currentRewardCount, rewardAmount);
            currentRewardCount -= amount;
            
            DOVirtual.DelayedCall(_spawnDelay * i, () =>
            {
                var item = ResourceController.GetInstance(_rewardType);
                item.SetAmount(amount);
                item.Claim();
                item.transform.position = _spawnPoint.position;
                item.transform.localScale = Vector3.one;
                Player.Stack.GetStack(item.Type).Add(item);
            }).SetId(this);
        }
    }
}
