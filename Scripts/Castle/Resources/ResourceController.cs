using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;
using Zenject;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private List<ResourceData> _resourcesData;
    [SerializeField] private Transform _poolParent;
    [SerializeField] private int _poolSize;

    [Inject] private DiContainer _container;

    private bool _intilialized = false;
    private Dictionary<ItemType, IPool<StackItem>> _itemPools = new();

    public Transform DefaultParent => _poolParent;
    public IEnumerable<ResourceData> Resources => _resourcesData;
    
    private Dictionary<ItemType, IPool<StackItem>> ItemsPool
    {
        get
        {
            if (_intilialized == false)
            {
                Initialize();
            }
            return _itemPools;
        }
    }
    
    private void Awake()
    {
        Initialize();
    }


    private void Initialize()
    {
        if(_intilialized)
            return;
        
        foreach (var resourceData in _resourcesData)
        {
            SimplePool<StackItem> simplePool = new SimplePool<StackItem>(resourceData.Prefab, _poolSize, _poolParent);
            simplePool.Initialize(_container);
            _itemPools.Add(resourceData.ItemType, simplePool);
        }
        
        _intilialized = true;
    }


    public Resource GetPrefab(ItemType itemType)
    {
        var resourceData = _resourcesData.Find(x => x.ItemType == itemType);
        return resourceData?.Prefab;
    }

    public StackItem GetInstance(ItemType itemType)
    {
        var item = ItemsPool[itemType].Get();
        item.transform.localScale = Vector3.one;
        return ItemsPool[itemType].Get();
    }

    public List<StackItem> GetInstances(ItemType itemType, int count, Action<StackItem> handle = null)
    {
        var pool = ItemsPool[itemType];
        List<StackItem> resources = new List<StackItem>();
        for (int i = 0; i < count; i++)
        {
            var resource = pool.Get();
            resources.Add(resource);
            handle?.Invoke(resource);
        }

        return resources;
    }
}
