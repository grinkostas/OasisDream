using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Zenject;

public class SkinShop : MonoBehaviour, ISkinnedChanger
{
    [SerializeField] private MonoPool<SkinSelectorItem> _skinItemViewsPool;
    [SerializeField] private List<SkinnedItem> _skinnedItems;
    [SerializeField] private ResourceView _resourceView;
    [SerializeField] private Button _buyButton;

    [Inject] private DiContainer _container;
    [Inject] private SkinManager _skinManager;
    [Inject] private Player _player;

    private List<SkinData> _allSkins = new List<SkinData>();
    private List<SkinData> AllSkins
    {
        get
        {
            if (_allSkins.Count == 0)
                _allSkins = _skinManager.GetAllSkins();
            return _allSkins;
        }
    }

    private List<SkinSelectorItem> _skinSelectorItems = new();

    private SkinData _currentSkinData;
    private SkinnedItem _targetSkinnedItem;


    public string ItemId => _targetSkinnedItem.ItemId;
    
    private void OnEnable()
    {
        _resourceView.Hide();
        _skinItemViewsPool.Initialize(_container);
        Actualize();
    }

    private void Actualize()
    {
        Refresh();
        foreach (var skinData in AllSkins)
        {
            var skinSelectorItem = _skinItemViewsPool.Get().Init(skinData);
            _skinSelectorItems.Add(skinSelectorItem);
            if(skinData.Available == false) 
                skinSelectorItem.Selected += OnItemSelected;
        }
    }

    private void OnDisable()
    {
        Refresh();
        foreach (var skinnedItem in _skinnedItems)
        {
            skinnedItem.ResetSelection();
        }
    }

    private void Refresh()
    {
        foreach (var skinSelectorItem in _skinSelectorItems)
        {
            skinSelectorItem.Pool.Return(skinSelectorItem);
            skinSelectorItem.Selected -= OnItemSelected;
        }
        _skinSelectorItems.Clear();
    }
    
    
    private void OnItemSelected(SkinSelectorItem selectedItem)
    {
        _currentSkinData = selectedItem.SkinData;
        _targetSkinnedItem = GetItemUser(_currentSkinData);
        
        ShowSkinDemo(_targetSkinnedItem, _currentSkinData);
        ActualizeBuyButton(selectedItem.SkinData);
    }

    private SkinnedItem GetItemUser(SkinData skinData)
    {
        string skinUser = _skinManager.GetSkinUser(skinData.Id);
        return _skinnedItems.Find(x => x.ItemId == skinUser);
    }
    
    private void ShowSkinDemo(SkinnedItem targetItem, SkinData skinData)
    {
        if(targetItem == null)
            return;
        
        targetItem.SelectSkin(skinData.Id);
    }
    
    private void ActualizeBuyButton(SkinData skinData)
    {
        var price = skinData.Price;
        _resourceView.Init(price.Resource, price.Amount);
        _buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    private void OnBuyButtonClick()
    {
        if(_currentSkinData == null)
            return;
        
        if (_player.Stack.MainStack.TrySpend(_currentSkinData.Price.Resource, _currentSkinData.Price.Amount))
        {
            Buy();
        }
    }

    private void Buy()
    {
        _currentSkinData.Buy();
        _targetSkinnedItem.EquipSkin(_currentSkinData.Id);
        _skinManager.ChangeSkin(this, _currentSkinData.Id);
        Actualize();
        _resourceView.Hide();
    }

}
