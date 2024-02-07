using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using Zenject;
using Random = UnityEngine.Random;

public class ResourceGenerateFx : MonoBehaviour
{
    [SerializeField] private ResourceGenerator _resourcePlaceGenerator;
    [SerializeField] private StackItemRotator _stackItemRotator;
    [SerializeField] private Vector2 _spawnRange;
    [SerializeField] private float _targetY;
    [SerializeField] private MoveFx _moveFx;
    [SerializeField] private bool _local = true;
    [SerializeField] private float _zoomInDuration;

    
    private void OnEnable()
    {
        _resourcePlaceGenerator.Spawned += OnResourceSpawned;
    }

    private void OnDisable()
    {
        _resourcePlaceGenerator.Spawned -= OnResourceSpawned;
    }

    private void OnResourceSpawned(StackItem collectable)
    {
        Vector2 randomPoint = Random.insideUnitCircle * _spawnRange.Random();
        Vector3 delta = new Vector3(randomPoint.x, _targetY, randomPoint.y);
        
        if (_local)
            collectable.transform.localPosition = Vector3.zero;
        else
            collectable.transform.position = transform.position;

        collectable.transform.DOScale(Vector3.one, _zoomInDuration);
        Vector3 destination = delta;
        if (_local == false)
            destination += collectable.transform.position;
        _moveFx.Move(collectable.transform, _resourcePlaceGenerator.Parent, destination, localMove:_local);
        _stackItemRotator.Rotate(collectable.transform, _moveFx.Duration);
        
        collectable.Claimed += OnClaim;
    }

    private void OnClaim(StackItem item)
    {
        item.Claimed -= OnClaim;
        DOTween.Kill(item.transform);
    }
    
}
