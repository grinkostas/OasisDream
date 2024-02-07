using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Haptic;
using StaserSDK.Stack;
using Zenject;

public class DropFx : MonoBehaviour
{
    [SerializeField] private StackProvider _stackProvider;
    [SerializeField] private Transform _defaultParent;
    [SerializeField] private StackItemRotator _stackItemRotator;
    [Header("Move")]
    [SerializeField] private MoveFx _moveFx;
    [Header("Zoom")] 
    [SerializeField] private float _zoomInDuration;

    [SerializeField] private bool _instanceStack;
    
    private void OnEnable()
    {
        _stackProvider.Interface.TookItem += MoveItem;
    }

    private void OnDisable()
    {
        _stackProvider.Interface.TookItem -= MoveItem;
    }

    public void MoveItem(StackItemData takeData)
    {
        Transform target = takeData.Target.transform;
        takeData.Target.Claim();
        
        if (_instanceStack == false)
        {
            target.SetParent(_defaultParent);
            target.localPosition = Vector3.zero;
        }
        
        target.SetParent(takeData.DestinationPoint);
        _moveFx.Move(target, takeData.DestinationPoint, takeData.Delta, changeParent: true, progress:takeData.Progress);
        target.localScale = Vector3.zero;
        
        if (takeData.OverrideRotation == false)
            _stackItemRotator.Rotate(target, _moveFx.Duration);
        else
        {
            if (_instanceStack == false)
                _stackItemRotator.Rotate(target, _moveFx.Duration, takeData.DestinationPoint.rotation.eulerAngles);
            else
                _stackItemRotator.Rotate(target, _moveFx.Duration, takeData.DestinationPoint);
        }
        
        target.gameObject.SetActive(true);
        target.DOScale(Vector3.one, _zoomInDuration).SetEase(Ease.OutBack);
    }
    
}
