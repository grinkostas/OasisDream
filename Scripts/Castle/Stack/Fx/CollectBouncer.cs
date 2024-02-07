using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using Zenject;

public class CollectBouncer : Bouncer
{
    [SerializeField] private float _bounceDelay;

    [Inject] private Player _player;

    private void OnEnable()
    {
        _player.Stack.MainStack.AddedItem += OnCountChanged;
    }
    
    private void OnDisable()
    {
        _player.Stack.MainStack.AddedItem -= OnCountChanged;
    }

    private void OnCountChanged(StackItemData data)
    {
        DOVirtual.DelayedCall(_bounceDelay, Bounce);
    }
}
