using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StaserSDK.Stack;
using Zenject;

public class Shop : MonoBehaviour
{
    [SerializeField] private MonoPool<ShopItemView> _shopItemViewMonoPool;
    [SerializeField] private ItemType _rewardResource;
    [SerializeField] private ResourceView _rewardPlayerResourceView;
    [SerializeField] private CanvasGroup _shopCanvasGroup;
    
    [Inject] private Player _player;
    [Inject] private DiContainer _container;
    [Inject] private ResourceController _resourceController;
    
    private List<ShopItemView> _shopItemViews = new List<ShopItemView>();
    public ItemType RewardResource => _rewardResource;
    public CanvasGroup CanvasGroup => _shopCanvasGroup;
    public ResourceView RewardPlayerResourceView => _rewardPlayerResourceView;
    
    private void Awake()
    {
        _shopItemViewMonoPool.Initialize(_container);
    }

    private void OnEnable()
    {
        RewardPlayerResourceView.Init(_player.Stack.MainStack, RewardResource);
        RewardPlayerResourceView.Actualize(RewardResource, _player.Stack.MainStack.Items[RewardResource].Value);
        ShowShopItems();
    }

    private void OnDisable()
    {
        foreach (var shopItemView in _shopItemViews)
        {
            _shopItemViewMonoPool.Return(shopItemView);
        }
    }

    private void ShowShopItems()
    {
        var stackItems = _player.Stack.MainStack.Items;
        foreach (var stackItem in stackItems)
        {
            if(stackItem.Value.Value <= 0)
                continue;
            
            if(_resourceController.GetPrefab(stackItem.Key).IsSellable == false)
                continue;
            
            var shopItem = _shopItemViewMonoPool.Get();
            shopItem.Init(stackItem.Key, this);
            _shopItemViews.Add(shopItem);
        }
    }
    
}
