using System;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Stack;
using UnityEngine;

namespace GameCore.Scripts.Stack
{
    public class CoinsCollectFx : MonoBehaviour
    {
        [Header("Stack")] 
        [SerializeField] private StackBase _stack;
        [Header("Make Sphere")]
        [SerializeField] private Vector2 _sphereRadiusRange;
        [SerializeField] private Vector3 _claimOffset;
        [SerializeField] private float _makeSphereDuration;
        [SerializeField] private Ease _makeSphereEase;
        [Space] 
        [SerializeField] private float _resetCointCountDelay = 2;
        [SerializeField] private int _maxCoinsCount = 50;
        [Header("Rotate")] 
        [SerializeField] private Vector2 _rotateRange;
        [SerializeField] private Vector3 _rotateAxis;
        [SerializeField] private Ease _rotateEase;
        [Header("LookAt")] 
        [SerializeField] private Ease _lookAtEase;
        [SerializeField] private float _lookAtTargetDuration;
        [Header("Move")]
        [SerializeField] private Ease _moveToTargetEase;
        [SerializeField] private float _receiveDestinationDuration;
        
        private float _latitudeIncrement = (Mathf.PI * 2) /1.618033f;

        private int _coinsCount = 0;

        private void OnEnable()
        {
            _stack.TookItem += MakeSphere;
        }

        private void OnDisable()
        {
            _stack.TookItem -= MakeSphere;
        }


        private void MakeSphere(StackItemData stackItem)
        {
            Transform target = stackItem.Target.transform;
            Transform destination = stackItem.DestinationPoint;
            _coinsCount++;
            if (_coinsCount > _maxCoinsCount)
                _coinsCount -= _maxCoinsCount;
            ResetBillsCount();

            var sequence = DOTween.Sequence();
            sequence.Append(SphereCreate(target, _coinsCount));
            sequence.Join(Rotate(target));
            sequence.Append(LookAt(target, destination, _lookAtTargetDuration));
            sequence.Append(LookAt(target, destination, _receiveDestinationDuration));
            sequence.Join(MoveToTarget(target, destination));
            sequence.OnComplete(() => stackItem.Target.Pool.Return(stackItem.Target));
        }

        private Tween SphereCreate(Transform target, int coinsCount)
        {
            int coefficient = coinsCount%2 == 0 ? -1 : 1;
            float latitude = coinsCount * _latitudeIncrement;
            float longitude = Mathf.Acos(1 - (coinsCount + 1) / (coinsCount + 1f));

            float sphereRadius = _sphereRadiusRange.Random();
            float x = sphereRadius * Mathf.Sin(longitude) * Mathf.Cos(latitude);
            float y = sphereRadius * Mathf.Sin(longitude) * Mathf.Sin(latitude);
            float z = sphereRadius * Mathf.Cos(longitude);

            Vector3 cubePosition = new Vector3(x, y, coefficient * z);
            return target.DOLocalMove(cubePosition + _claimOffset, _makeSphereDuration).SetEase(_makeSphereEase);
        }
        
        private Tween Rotate(Transform target)
        {
            float x = _rotateRange.Random() * _rotateAxis.x;
            float y = _rotateRange.Random() * _rotateAxis.y;
            float z = _rotateRange.Random() * _rotateAxis.z;
            var targetRotation = target.localRotation.eulerAngles + new Vector3(x, y, z);
            return target.DOLocalRotate(targetRotation, _makeSphereDuration).SetEase(_rotateEase);
        }

        private Tween LookAt(Transform target, Transform destinationPoint, float duration)
        {
            var startRotation = target.rotation;
            bool initialized = false;
            return DOVirtual.Float(0, 1, duration, value =>
            {
                if (initialized == false)
                {
                    startRotation = target.rotation;
                    initialized = true;
                }
                Vector3 relativePosition = destinationPoint.position - target.position;
                var destinationRotation = Quaternion.LookRotation(relativePosition);
                var currentRotation = Quaternion.Lerp(startRotation, destinationRotation, value);
                target.rotation = currentRotation;
            }).SetEase(_lookAtEase);
        }

        private Tween MoveToTarget(Transform target, Transform destination)
        {
            var startPosition = target.position;
            bool initialized = false;
            return DOVirtual.Float(0, 1, _receiveDestinationDuration, value =>
            {
                if (initialized == false)
                {
                    startPosition = target.position;
                    initialized = true;
                }
                target.position = Vector3.Lerp(startPosition, destination.position, value);
            }).SetEase(_moveToTargetEase);
        }
        
        private void ResetBillsCount()
        {
            DOTween.Kill(this);
            DOVirtual.DelayedCall(_resetCointCountDelay, () => _coinsCount = 0);
        }
    }
}