using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using StaserSDK.Stack;

public class StackWiggle : StackWiggleEffect
{
    [SerializeField] private List<StackWiggleEffect> _stackWiggleEffects;
    [SerializeField] private CharacterStackLocator _stackLocator;
    [SerializeField] private InstanceCollectFx _instanceCollectFx;
    
    [SerializeField] private float _dropMoveTime;

    private List<StackItem> _ignoreItems = new();

    private void OnEnable()
    {
        _stackLocator.Stack.AddedItem += OnAddItem;
        _stackLocator.Stack.TookItem += OnTakeItem;
    }

    private void OnDisable()
    {
        _stackLocator.Stack.AddedItem -= OnAddItem;
        _stackLocator.Stack.TookItem -= OnTakeItem;
    }

    private void OnTakeItem(StackItemData itemData)
    {
        itemData.Target.Wrapper.DOLocalMove(Vector3.zero, _dropMoveTime);
    }
    
    private void OnAddItem(StackItemData itemData)
    {
        var stackItem = itemData.Target;
        var startPosition = stackItem.Wrapper.localPosition;
        _ignoreItems.Add(stackItem);
        
        DOVirtual.Float(0, 1, _instanceCollectFx.MoveFx.Duration, value =>
        {
            int row = _stackLocator.GetRow(stackItem);
            int column = _stackLocator.GetColumn(stackItem);
            itemData.Target.Wrapper.localPosition = Vector3.Lerp(startPosition, GetOffset(row, column), value);
        }).OnComplete(()=> _ignoreItems.Remove(stackItem));
    }

    public override Vector3 GetOffset(int row, int column)
    {
        Vector3 offset = Vector3.zero;
        
        foreach (var wiggleEffect in _stackWiggleEffects)
            offset += wiggleEffect.GetOffset(row, column);

        return offset;
    }

    private void Update()
    {
        var sourceItems = _stackLocator.Stack.SourceItems;
        for (int i = 0; i < sourceItems.Count; i++)
        {
            if(_ignoreItems.Contains(sourceItems[i]))
                continue;
            sourceItems[i].Wrapper.localPosition = GetOffset(_stackLocator.GetRow(i), _stackLocator.GetColumn(i));
        }
    }
}
