using System;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using StaserSDK.Stack;
using UnityEngine;

namespace GameCore.Scripts.Stack
{
    public class StackItemLocatorPlacer : MonoBehaviour
    {
        [SerializeField] private StackBase _targetStack;
        [SerializeField] private StackItemLocator _stackItemLocator;
        [SerializeField] private float _zoomTime;
        [SerializeField] private float _moveTime;
        [SerializeField] private float _ySpawnDelta;
        
        private void OnEnable()
        {
            _targetStack.AddedItem += OnAddItem;
        }

        private void OnDisable()
        {
            _targetStack.AddedItem -= OnAddItem;
        }

        private void OnAddItem(StackItemData addData)
        {
            Transform target = addData.Target.transform;
            Vector3 targetDestination = _stackItemLocator.GetPostDelta();
            
            target.localScale = Vector3.zero;
            target.localPosition = targetDestination + Vector3.up * _ySpawnDelta;
            
            target.DOLocalMoveY(targetDestination.y, _moveTime);
            target.DOScale(Vector3.one, _zoomTime).SetEase(Ease.OutBack);
        }
    }
}