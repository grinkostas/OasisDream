using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using StaserSDK.Extentions;
using StaserSDK.Stack;
using UnityEngine.Events;
using Zenject;

public class HiddenStack : StackBase
{
    [Inject] private ResourceController _resourceController;

    protected override bool TryGetInstance(ItemType type, out StackItem stackItem)
    {
        stackItem = _resourceController.GetInstance(type);
        stackItem.Claim();
        stackItem.transform.position = transform.position;
        return true;
    }

    protected override void OnAddItem(StackItemData stackItemData)
    {
        var stackItem = stackItemData.Target;
        if (stackItemData.SkipAnimation)
        {
            stackItem.Pool.Return(stackItem);
            return;
        }
        SourceItems.Remove(stackItem);
        DOVirtual.DelayedCall(2.0f, ()=> stackItem.Pool.Return(stackItem));
    }

    protected override IEnumerable<StackItem> GetInstances(ItemType type, int count)
    {
        return _resourceController.GetInstances(type, count);
    }
}
