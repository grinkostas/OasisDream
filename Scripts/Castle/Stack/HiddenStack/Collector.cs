using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using Zenject;

[RequireComponent(typeof(Collider))]
public class Collector : MonoBehaviour
{
    [SerializeField] private StackProvider _stackProvider;
    [SerializeField] private float _collectDelay;

    [Inject] private Timer _timer;
    private float _waitedTime = 0.0f;
    
    public List<StackItem> _takeQueue = new List<StackItem>();

    public void Clear()
    {
        _takeQueue.Clear();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StackItem collectable))
        {
            if(_takeQueue.Contains(collectable))
                return;
            if(collectable.IsClaimed)
                return;
            _takeQueue.Add(collectable);
            collectable.Claimed += OnClaimed;
            collectable.UnClaimed += OnClaimed;
        }
    }

    private void OnClaimed(StackItem item)
    {
        _takeQueue.Remove(item);
        item.Claimed -= OnClaimed;
        item.UnClaimed -= OnClaimed;
    }

    private void Collect(StackItem collectable)
    {
        if (_stackProvider.Interface.Disabled)
            return;
        
        _takeQueue.Remove(collectable);
        if(collectable.IsClaimed)
            return;
        
        collectable.Claim();
        if (_stackProvider.Interface.TryAdd(collectable) == false)
        {
            collectable.UnClaim();
        }
    }

    private void Update()
    {
        if(_takeQueue.Count == 0)
            return;
        
        if (_stackProvider.Interface.Disabled)
            return;
        
        _waitedTime += Time.deltaTime;
        if(_waitedTime < _collectDelay)
            return;
        _waitedTime = 0.0f;
        while (_takeQueue.Count > 0)
        {
            var takeItem = _takeQueue[0];
            if (takeItem.IsClaimed || takeItem.gameObject.activeInHierarchy == false)
            {
                _takeQueue.Remove(takeItem);
                continue;
            }

            Collect(takeItem);
        }
        
    }
}
