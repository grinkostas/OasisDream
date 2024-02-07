using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Zenject;

public class SkinSelectView : View
{
    [SerializeField] private SkinItemView _skinItemView;
    [SerializeField] private MonoPool<SkinSelectorItem> _skinSelectorItemsPool;

    [Inject] private SkinManager _skinManager;
    [Inject] private DiContainer _container;

    private List<SkinSelectorItem> _skinSelectorItems = new();
    
    public override bool IsHidden => enabled;
    public string ItemId => _skinItemView.ItemId;
    
    public event UnityAction<SkinData> Selected;
    
    public override void Show()
    {
        _skinSelectorItemsPool.Initialize(_container);
        gameObject.SetActive(true);
        var availableSkins = _skinManager.GetAvailableSkins(ItemId);
        foreach (var availableSkin in availableSkins)
            InitializeItem(availableSkin);
    }

    private void InitializeItem(SkinData availableSkin)
    {
        var selectorItem = _skinSelectorItemsPool.Get();
        selectorItem.Init(availableSkin);
        selectorItem.Selected += OnSkinSelect;
        _skinSelectorItems.Add(selectorItem);
    }

    private void OnSkinSelect(SkinSelectorItem item)
    {
        if(item.Initialized == false)
            return;
        Selected?.Invoke(item.SkinData);
        _skinItemView.Actualize();
    }

    public override void Hide()
    {
        foreach (var selectorItem in _skinSelectorItems)
        {
            selectorItem.Selected -= OnSkinSelect;
            selectorItem.Pool.Return(selectorItem);
        }
        _skinSelectorItems.Clear();
        gameObject.SetActive(false);
    }
}
