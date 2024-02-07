using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.Scripts.Utilities;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace GameCore.Scripts.Stack
{
    public class ResourceCanvasFx : MonoBehaviour
    {
        [SerializeField] private MonoPool<CanvasItem> _pool;
        [SerializeField] private int _maxCount;
        [SerializeField] private float _circleRadius;
        [SerializeField] private float _makeCircleTime;
        [SerializeField] private float _inTime;
        [SerializeField] private float _outTime;
        [SerializeField] private float _moveTime;
        [SerializeField] private float _startMoveDelta;

        [Inject, UsedImplicitly] public OverlayCanvas OverlayCanvas { get; }
        [Inject, UsedImplicitly] public SoftCurrencyDisplay SoftCurrencyDisplay { get; }
        [Inject, UsedImplicitly] public Camera Camera { get; }
        [Inject, UsedImplicitly] public Player Player { get; }

        public Vector3 DefaultDestination => SoftCurrencyDisplay.IconImage.transform.position;

        [Inject]
        private void OnInject()
        {
            _pool.Initialize();
        }
       
        public void Visualize(Vector3 startPoint, Vector3 destination, int count, bool needToConvertSpace = false, UnityAction<int> onReceiveDestination = null)
        {
            int instancesCount = Math.Min(count, _maxCount);
            int currentCount = count;
            int itemAmount = Mathf.CeilToInt(count / (float)instancesCount);
            
            for(int i = 0; i < instancesCount; i++)
            {
                var item = PrepareItemAnimation(startPoint, needToConvertSpace);
                int amount = Mathf.Min(currentCount, itemAmount);
                currentCount -= amount;
                
                AnimateItem(item, destination, _startMoveDelta * i, () =>
                {
                    onReceiveDestination?.Invoke(amount);
                });
            }
        }

        private CanvasItem PrepareItemAnimation(Vector3 startPoint, bool convert = false)
        {
            var item = _pool.Get();
            item.CanvasGroup.alpha = 0;

            if (convert == false)
            {
                item.transform.position = startPoint;
                return item;
            }

            var viewportPoint = Camera.WorldToViewportPoint(startPoint);
            item.Rect.anchorMax = item.Rect.anchorMin = viewportPoint;
            
            return item;
        }
        

        private Tween AnimateItem(CanvasItem item, Vector3 destination, float delay, UnityAction onReceiveDestination)
        {
            var randomDelta = Random.insideUnitCircle * _circleRadius;
            var targetTransform = item.transform;
            
            var sequence = DOTween.Sequence();
            sequence
                .Append(targetTransform.DOLocalMove(item.transform.localPosition + randomDelta.XY(), _makeCircleTime).SetEase(Ease.OutBack))
                .Join(item.CanvasGroup.DOFade(1, _inTime))
                .Join(targetTransform.DOScale(1, _inTime).SetEase(Ease.OutBack))
                .AppendInterval(delay)
                .Append(targetTransform.DOMove(destination, _moveTime).SetEase(Ease.InBack))
                .Join(item.CanvasGroup.DOFade(1, _outTime).SetDelay(_moveTime-_outTime))
                .Join(targetTransform.DOScale(0, _outTime).SetDelay(_moveTime-_outTime))
                .OnComplete(() =>
                {
                    item.Pool.Return(item);
                    onReceiveDestination?.Invoke();
                });
            return sequence;
        }
        
    }
}