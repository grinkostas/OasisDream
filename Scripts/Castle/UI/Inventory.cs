using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;
using Zenject;

public class Inventory : MonoBehaviour
{
    [SerializeField] private MonoPool<ResourceView> _viewsPool;

    [Inject] private Player _player;
    [Inject] private DiContainer _container;

    private List<ResourceView> _views = new List<ResourceView>();

    private void OnEnable()
    {
        _viewsPool.Initialize(_container);
        foreach (var stackItem in _player.Stack.MainStack.Items)
        {
            if(stackItem.Key is ItemType.Any or ItemType.None)
                continue;
            if(stackItem.Value.Value <= 0)
                continue;
            var view = _viewsPool.Get().Init(stackItem.Key, stackItem.Value.Value);
            _views.Add(view);
        }
    }

    private void OnDisable()
    {
        foreach (var view in _views)
        {
            view.Pool.Return(view);
        }
        _views.Clear();
    }
}
