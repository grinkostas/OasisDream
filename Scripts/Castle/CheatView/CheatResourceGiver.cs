using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;
using UnityEngine.UI;
using Zenject;

public class CheatResourceGiver : MonoBehaviour
{
    [SerializeField] private ItemsTypeDropdown _dropdown;
    [SerializeField] private CheatCountView _countView;
    [SerializeField] private Button _getButton;
    [SerializeField] private int _maxItemsInStack = 75;

    [Inject] private Player _player;
    [Inject] private ResourceController _resourceController;

    private IStack Stack => _player.Stack.MainStack;
    
    private void OnEnable()
    {
        _getButton.onClick.AddListener(OnGetButtonClick);
    }
    private void OnDisable()
    {
        _getButton.onClick.RemoveListener(OnGetButtonClick);
    }

    private void OnGetButtonClick()
    {
        if(_countView.CurrentValue <= 0 || _resourceController.GetPrefab(_dropdown.CurrentItemType) == null)
            return;

        if (_dropdown.CurrentItemType == ItemType.Diamond)
        {
            _player.Stack.SoftCurrencyStack.TryAddRange(ItemType.Diamond, _countView.CurrentValue);
            return;
        }

        int count = Math.Min(_countView.CurrentValue, _maxItemsInStack - Stack.ItemsCount);
        if(count == 0)
            return;
        
        for (int i = 0; i < count; i++)
        {
            var item = _resourceController.GetInstance( _dropdown.CurrentItemType);
            item.gameObject.SetActive(true);
            Stack.Add(item);
        }
    }
}
