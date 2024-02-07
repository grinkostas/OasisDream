using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using Zenject;

public class ShopItemViewFx : MonoBehaviour
{
    [SerializeField] private ShopItemView _shopItemView;
    [SerializeField] private RectTransform _model;
    [SerializeField] private Puncher _puncher;
    [SerializeField] private float _fxTargetScale = 66;
    [SerializeField] private Transform _fxParent;
    private CanvasGroup ShopCanvasGroup => _shopItemView.Shop.CanvasGroup;

    [Inject] private ResourcesFx _resourcesFx;
    [Inject] private ResourceController _resourceController;
    
    private int _waitedReward;
    private int _iterationReward;
    
    private void OnEnable()
    {
        _shopItemView.Sold += OnSold;
    }

    private void OnDisable()
    {
        _shopItemView.Sold -= OnSold;
    }

    private void OnSold(int reward)
    {
        ShopCanvasGroup.interactable = false;
        _waitedReward = reward;
        int visualizeReward = Mathf.Clamp(reward, 1, 10);

        int particlesCount = _resourcesFx.Visualize(
            _resourceController.GetInstances(_shopItemView.Shop.RewardResource, visualizeReward),
            _shopItemView.Shop.RewardPlayerResourceView.IconImage.transform.position,
            _shopItemView.Shop.transform,
            _fxParent.position,
            ActualizeResourceView, 
            _fxTargetScale);

        _iterationReward = Mathf.CeilToInt(reward / (float)particlesCount);

        _puncher.Punch(_model).OnComplete(_shopItemView.Actualize);
    }

    private void ActualizeResourceView()
    {
        if(_waitedReward <= 0)
            return;
        
        _waitedReward -= _iterationReward;
        
        var resourceText = _shopItemView.Shop.RewardPlayerResourceView.AmountText;
        int startCount = Int32.Parse(resourceText.text);
        int totalCount = startCount + _iterationReward;
        if (_waitedReward <= _iterationReward)
        {
            ShopCanvasGroup.interactable = true;
            totalCount += _waitedReward;
        }
        
        _shopItemView.Shop.RewardPlayerResourceView.Actualize(_shopItemView.Shop.RewardResource, totalCount);
    }

}
