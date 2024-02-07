using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using StaserSDK.Stack;
using UnityEngine.Events;
using Zenject;

public abstract class ResourceGenerator : MonoBehaviour
{
    [Inject, UsedImplicitly] public ResourceController ResourceController { get; }
    
    public UnityAction<StackItem> Spawned { get; set; }
    public abstract Transform Parent { get; }

    protected void SpawnResource(ItemType type)
    {
        var pooledResource = ResourceController.GetInstance(type);
        pooledResource.Restore();
        pooledResource.transform.SetParent(Parent, true);
        SetStartPosition(pooledResource);
        pooledResource.gameObject.SetActive(true);
        Spawned?.Invoke(pooledResource);
    }

    protected virtual void SetStartPosition(StackItem stackItem)
    {
        stackItem.transform.localPosition = Vector3.zero;
    }
}
