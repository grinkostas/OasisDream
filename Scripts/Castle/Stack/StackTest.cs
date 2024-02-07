using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using StaserSDK.Stack;
using Zenject;

public class StackTest : MonoBehaviour
{
    [SerializeField] private List<ItemType> _typeToStore;
    [SerializeField] private int _itemsCount;
    [Inject, UsedImplicitly] public ResourceController resourceController { get; }
    
    private void Start()
    {
        Vector3 yDelta = Vector3.zero;

        foreach (var type in _typeToStore)
        {
            for (int i = 0; i < _itemsCount/_typeToStore.Count; i++)
            {
                var resource = resourceController.GetPrefab(type);
                Debug.Log(resource == null);
                var res = Instantiate(resource, transform);
                res.transform.localPosition = yDelta;
                yDelta += Vector3.Scale(Vector3.up, resource.StackSize.Size); 
            }
        }
        
    }
}
