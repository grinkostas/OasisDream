using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Haptic;
using StaserSDK.Interactable;
using StaserSDK.Stack;
using UnityEngine.Events;
using Zenject;

public class ShopSpot : MonoBehaviour
{
    [SerializeField] private StackableCharacterZone _zone;
    [SerializeField] private Transform _takePoint;
    [SerializeField] private float _diamondSpawnDelay;
    [SerializeField] private StackBase _stack;
    [SerializeField] private ItemType _rewardType = ItemType.Diamond;

    [Inject] private IHapticService _hapticService;
    [Inject] private ResourceController _resourceController;
    [Inject] private Player _player;

    private StackableCharacter _stackableCharacter => _player.Stack;
    
    private int _delayedSpawnCount = 0;
    public ZoneBase TakeZone => _zone;
    public UnityAction<StackItem> Sold { get; set; }
    
    private void OnEnable()
    {
        _zone.OnEnterInternal += OnEnter;
        _zone.OnExitInternal += OnExit;
        _zone.OnInteractInternal += OnInteract;
    }

    private void OnDisable()
    {
        _zone.OnEnterInternal -= OnEnter;
        _zone.OnExitInternal -= OnExit;
        _zone.OnInteractInternal -= OnInteract;
    }

    private void OnInteract(StackableCharacter character)
    {
        if(character.MainStack.ItemsCount <= 0)
            return;
        
        if(character.MainStack.TryGetLastType(out ItemType takeType) == false)
            return;
        
        if(_stack.ItemsCount + _delayedSpawnCount + _resourceController.GetPrefab(takeType).SellPrice > _stack.MaxSize)
            return;
        
        if (character.MainStack.TryTakeLast(out StackItem stackItem, _takePoint) == false)
            return;
        
        stackItem.Claim();
        _hapticService.Selection();
        _delayedSpawnCount += stackItem.SellPrice;
        Sold?.Invoke(stackItem);
        DOVirtual.DelayedCall(_diamondSpawnDelay, () => TakeItem(stackItem));
    }

    private void TakeItem(StackItem stackItem)
    {
        for (int i = 0; i < stackItem.SellPrice; i++)
        {
            _delayedSpawnCount--;
            var rewardItem = _resourceController.GetInstance(_rewardType);
            
            rewardItem.transform.SetParent(_stack.gameObject.transform, false);
            rewardItem.Claim();
            if(_stack.TryAdd(rewardItem) == false)
                break;
        }
        stackItem.Pool.Return(stackItem);
    }
    
    private void OnEnter(StackableCharacter interactableCharacter)
    {
        interactableCharacter.MainStack.DisableCollect(this);
        interactableCharacter.SoftCurrencyStack?.DisableCollect(this);
        EnableCollect();
    }

    private void EnableCollect()
    {
        DOVirtual.DelayedCall(2, () =>
        {
            if (_zone.IsCharacterInside == false)
                OnExit(_stackableCharacter);
            EnableCollect();
        });

    }
        
    private void OnExit(StackableCharacter interactableCharacter)
    {
        interactableCharacter.MainStack.EnableCollect(this);
        interactableCharacter.SoftCurrencyStack?.EnableCollect(this);
    }
}
