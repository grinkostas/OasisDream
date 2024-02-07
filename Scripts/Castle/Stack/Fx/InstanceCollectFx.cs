using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NepixSignals;
using StaserSDK.Stack;

public class InstanceCollectFx : MonoBehaviour
{
    [SerializeField] private StackProvider _stackProvider;
    [Header("Move")]
    [SerializeField] private MoveFx _moveFx;
    [SerializeField] private CharacterStackLocator _stackItemLocator;
    [Header("Rotation")]
    [SerializeField] private StackItemRotator _stackRotator;

    public MoveFx MoveFx => _moveFx;

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
        _moveFx.Move(target.transform, _stackItemLocator.transform, _stackItemLocator.GetCurrentDelta(), changeParent:true)
            .OnComplete(() => target.transform.localRotation = Quaternion.identity);
        _stackRotator.Rotate(target.transform, _moveFx.Duration, _stackItemLocator.transform);
        target.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
    }
}
