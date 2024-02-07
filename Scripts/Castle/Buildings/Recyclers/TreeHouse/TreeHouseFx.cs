using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using Zenject;

public class TreeHouseFx : RecycleAdditionalFx
{
    [Header("Saw")]
    [SerializeField] private Rotator _sawRotator;
    [Header("Base Move")]
    [SerializeField] private Transform _woodWrapper;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _destination;
    [SerializeField] private float _moveTime;
    [Header("Left Part")]
    [SerializeField] private Transform _leftWoodPart;
    [SerializeField] private Vector3 _leftPartMoveDelta;
    [SerializeField] private Vector3 _leftPartRotationDelta;
    [Header("Right Part")]
    [SerializeField] private Transform _rightWoodPart;
    [SerializeField] private Vector3 _rightPartMoveDelta;
    [SerializeField] private Vector3 _rightPartRotationDelta;
    [Header("Slicing")]
    [SerializeField] private float _sliceDelay;
    [SerializeField] private float _sliceDuration;
    [Header("Zoom")]
    [SerializeField] private float _zoomDelay;
    [SerializeField] private float _zoomTime;

    private void Awake()
    {
        _woodWrapper.gameObject.SetActive(false);
    }

    protected override void OnEnableInternal()
    {
        _sawRotator.enabled = false;
    }

    protected override void OnStartRecycle()
    {
        _sawRotator.enabled = true;
    }

    protected override void ProductCycle()
    {
        Restore();
        _woodWrapper.gameObject.SetActive(true);
        _woodWrapper.DOMove(_destination.position, _moveTime);
        SliceItem(_leftWoodPart, _leftPartMoveDelta, _leftPartRotationDelta);
        SliceItem(_rightWoodPart, _rightPartMoveDelta, _rightPartRotationDelta);
    }

    private void SliceItem(Transform targetItem, Vector3 moveDelta, Vector3 rotationDelta)
    {
        targetItem.DOLocalMove(moveDelta, _sliceDuration).SetDelay(_sliceDelay);
        targetItem.DOScale(Vector3.zero, _zoomTime).SetDelay(_zoomDelay);
        targetItem.DOLocalRotate(rotationDelta, _sliceDuration).SetDelay(_sliceDelay);
    }

    private void Restore()
    {
        _woodWrapper.position = _startPoint.position;
        RestoreItem(_leftWoodPart);
        RestoreItem(_rightWoodPart);
    }

    private void RestoreItem(Transform targetItem)
    {
        targetItem.localScale = Vector3.one;
        targetItem.localRotation = Quaternion.identity;
        targetItem.localPosition = Vector3.zero;
    }

    protected override void OnEndRecycle()
    {
        _sawRotator.enabled = false;
    }
}
