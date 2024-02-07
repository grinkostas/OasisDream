using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StaserSDK.Stack;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class ShopItemView : MonoBehaviour, IPoolItem<ShopItemView>
{
    [SerializeField] private Image _resourceIcon;
    [SerializeField] private TMP_Text _selectedResourceAmount;
    [SerializeField] private Image _rewardIcon;
    [SerializeField] private TMP_Text _rewardAmount;
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _sellButton;
    
    [Inject] private ResourceController _resourceController;
    [Inject] private Player _player;

    
    
    private Resource _resource;
    private Resource _rewardResource;
    
    private int _maxValue = 1;

    private int SellAmount => Mathf.CeilToInt(_maxValue * _slider.value);
    
    public Shop Shop { get; private set; }
    public IPool<ShopItemView> Pool { get; set; }
    public bool IsTook { get; set; }

    public UnityAction<int> Sold;
    
    private void OnEnable()
    {
        _sellButton.onClick.AddListener(SellItems);
        _slider.onValueChanged.AddListener(OnChangeSliderValue);
    }
    private void OnDisable()
    {
        _sellButton.onClick.RemoveListener(SellItems);
    }

    public void Init(ItemType resourceType, Shop shop)
    {
        _resource = _resourceController.GetPrefab(resourceType);
        _resourceIcon.sprite = _resource.Icon;
        _rewardResource = _resourceController.GetPrefab(shop.RewardResource);
        _rewardIcon.sprite = _rewardResource.Icon;
        Shop = shop;
        Actualize();
    }

    public void Actualize()
    {
        _maxValue = _player.Stack.MainStack.Items[_resource.Type].Value;
        if (_maxValue == 0)
        {
            Pool.Return(this);
        }
        OnChangeSliderValue(_slider.value);
    }

    private void OnChangeSliderValue(float value)
    {
        _slider.value = value;
        _selectedResourceAmount.text = (SellAmount).ToString();
        _rewardAmount.text = (SellAmount * _resource.SellPrice).ToString();
    }

    private void SellItems()
    {
        if (_player.Stack.MainStack.TrySpend(_resource.Type, SellAmount) == false)
            return;
        int rewardAmount = SellAmount * _resource.SellPrice;
        _player.Stack.MainStack.TryAddRange(_rewardResource.Type, rewardAmount);
        Sold?.Invoke(rewardAmount);
    }


    public void TakeItem()
    {
    }

    public void ReturnItem()
    {
    }
}
