using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.Scripts.Stack;
using NaughtyAttributes;
using StaserSDK.Stack;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class UpgradeViewFx : MonoBehaviour
{
    [SerializeField] private UpgradeView _upgradeView;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Transform _destinationPoint;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private float _targetScale = 0.35f;
    [SerializeField] private View _closeButtonZoomView;
    [Header("Fxs")]
    [SerializeField] private RectTransform _model;
    [SerializeField] private Puncher _puncher;
    [SerializeField] private Bouncer _bouncer;
    
    [Inject] private Player _player;
    [Inject] private ResourcesFx _resourcesFx;
    [Inject] private ResourceCanvasFx _canvasFx;
    [Inject] private ResourceController _resourceController;

    private int _waitCount = 0;
    private int _iterationPriceDecrease = 0;
    private int _price = 0;
    
    private void OnEnable()
    {
        _upgradeView.Upgraded += OnUpgrade;
    }

    private void OnDisable()
    {
        _upgradeView.Upgraded -= OnUpgrade;
    }
    

    [Button("Test")]
    private void OnUpgrade()
    {
        _puncher.Punch(_model);
        _puncher.Punch(_player.Model);
        IEnumerable<StackItem> stackItems = _resourceController.GetInstances(ItemType.Diamond, 30, item => item.Pool.Return(item));
        _waitCount = _resourcesFx.Visualize(stackItems, _destinationPoint.position, OnFxReceiveDestination, _targetScale);
        _closeButtonZoomView.Hide();
        _price = Int32.Parse(_priceText.text);
        _iterationPriceDecrease = _price / _waitCount;
        
    }
    
    private void OnFxReceiveDestination()
    {
        _waitCount--;
        if (_waitCount == 0)
        {
            _puncher.Punch(_model);
            _bouncer.Bounce();
            _upgradeView.Actualize();
            _closeButtonZoomView.Show();
            return;
        }
        
        _price -= _iterationPriceDecrease;
        _priceText.text = _price.ToString();
        _bouncer.Bounce();
    }
}
