using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Haptic;
using NaughtyAttributes;
using StaserSDK.Interactable;
using StaserSDK.Stack;
using Zenject;

public class StackGiver : MonoBehaviour
{
    [SerializeField] private StackProvider _stackProvider;
    [SerializeField] private StackableCharacterZone _zoneBase;
    [SerializeField] private bool _haveTargetType;
    [SerializeField, ShowIf(nameof(_haveTargetType))] private ItemType _targetType;
    [SerializeField] private bool _overrideTransforms;
    [SerializeField, ShowIf(nameof(_overrideTransforms))] private Transform _parent;
    [SerializeField] private bool _skipAnimation;
    [SerializeField] private float _addDelay;

    [Inject] private IHapticService _hapticService;
    
    private void OnEnable()
    {
        _zoneBase.OnInteractInternal += OnInteract;
    }
    
    private void OnDisable()
    {
        _zoneBase.OnInteractInternal -= OnInteract;
    }

    protected void OnInteract(StackableCharacter character)
    {
        if(_stackProvider.Interface.ItemsCount == 0)
            return;
        
        var targetType = _haveTargetType ? _targetType : ItemType.Any;
        IStack stack = character.GetStack(targetType);
        
        if(stack.ItemsCount >= stack.MaxSize)
            return;
        
        StackItem stackItem;
        if(_stackProvider.Interface.TryTake(targetType, out stackItem, character.transform) == false)
            return;
        
        stackItem.gameObject.SetActive(true);
        if (_overrideTransforms)        
        {
            stackItem.transform.SetParent(_parent);
            stackItem.transform.localPosition = Vector3.zero;
        }

        DOVirtual.DelayedCall(_addDelay, () => stack.Add(stackItem, _skipAnimation));
        
        _hapticService.Selection();
    }
}
