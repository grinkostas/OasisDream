using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Stack;

public class CollectFx : MonoBehaviour
{
    [SerializeField] private StackProvider _stackProvider;
    [Header("Move")]
    [SerializeField] private MoveFx _moveFx;
    [SerializeField] private Transform _parentPoint;
    [Header("Rotation")]
    [SerializeField] private StackItemRotator _stackRotator;
    [Header("Zoom")] 
    [SerializeField] private Vector3 _targetZoomOut;
    [SerializeField] private float _zoomDuration;

    private void OnEnable()
    {
        _stackProvider.Interface.AddedItem += MoveItem;
    }

    private void OnDisable()
    {
        _stackProvider.Interface.AddedItem -= MoveItem;
    }

    public void MoveItem(StackItemData addData)
    {
        StackItem target = addData.Target;
        if (addData.SkipAnimation)
        {
            target.gameObject.SetActive(false);
            target.transform.SetParent(_parentPoint);
            target.transform.localPosition = Vector3.zero;
            target.Pool.Return(target);
        }
        _moveFx.Move(target.transform, _parentPoint, changeParent:true);
        _stackRotator.Rotate(target.transform, _moveFx.Duration);
        target.transform.DOScale(_targetZoomOut, _zoomDuration).SetDelay(_moveFx.Duration - _zoomDuration).SetEase(Ease.InBack)
            .OnComplete(() => target.gameObject.SetActive(false));
    }
}
