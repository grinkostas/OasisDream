using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using Zenject;
using Random = UnityEngine.Random;

public class ResourcesFx : MonoBehaviour
{
    [SerializeField] private RectTransform _spawnPoint;
    [SerializeField] private Transform _fxParent;
    [SerializeField] private int _createCount;
    [Header("Position")]
    [SerializeField] private float _spawnRange;

    [SerializeField] private float _moveTime;
    [Header("Rotation")]
    [SerializeField] private Vector2 _spawnRotateRange;
    [SerializeField] private Vector3 _rotateAxis;
    [SerializeField] private float _waitRotateAngle;
    [Header("Timings")]
    [SerializeField] private float _spawnMoveTime;
    [SerializeField] private float _baseStartMoveDelay;
    [SerializeField] private float _itemStartMoveDelay;
    [SerializeField] private float _zoomInDuration;
    [SerializeField] private float _zoomOutDuration;

    [Inject] private Player _player;

    public Transform FxParent => _fxParent;

    public int Visualize(IEnumerable<StackItem> stackItems, Vector3 destination, Transform senderParent, Vector3 startPosition = new Vector3(),
        Action onReceiveDestination = null, float targetScale = 1.0f)
    {
        int count = 0;
        foreach (var stackItem in stackItems)
        {
            if(count >= _createCount)
                break;
            count++;
            var stackItemTransform = stackItem.transform;
            
            InitializeItem(stackItemTransform, senderParent, targetScale);
            StartAnimation(startPosition, stackItemTransform, count);
            MoveToDestination(stackItemTransform, destination, count, onReceiveDestination);
        }

        return count;
    }
    
    public int Visualize(IEnumerable<StackItem> stackItems, Vector3 destination, Action onReceiveDestination = null, float targetScale = 1.0f)
    {
        return Visualize(stackItems, destination, _spawnPoint, _spawnPoint.position, onReceiveDestination, targetScale:targetScale);
    }
    
    public int Visualize(IEnumerable<StackItem> stackItems, Vector3 startPosition, Vector3 destination, Action onReceiveDestination = null, float targetScale = 1.0f)
    {
        return Visualize(stackItems, destination, _fxParent, startPosition, onReceiveDestination, targetScale:targetScale);
    }

    private void InitializeItem(Transform stackItemTransform, Transform parent, float targetScale = 1.0f)
    {
        stackItemTransform.SetParent(parent);
        stackItemTransform.transform.localScale = Vector3.zero;
        stackItemTransform.DOScale(Vector3.one * targetScale, _zoomInDuration);
        stackItemTransform.gameObject.SetActive(true);
        
    }

    private void StartAnimation(Vector3 startPosition, Transform stackItemTransform, int count)
    {
        Vector3 randomDelta = (Random.insideUnitCircle * _spawnRange);
        Vector3 showPosition = startPosition + randomDelta;
        stackItemTransform.position = showPosition;
        stackItemTransform.DOMove(showPosition + randomDelta, _spawnMoveTime);
        
        
        float randomAngle = _spawnRotateRange.Random();
        stackItemTransform.rotation = Quaternion.Euler(_rotateAxis * randomAngle);
        stackItemTransform.DORotate(_rotateAxis * (randomAngle+_waitRotateAngle) , _moveTime+_baseStartMoveDelay);
    }

    private void MoveToDestination(Transform stackItemTransform, Vector3 destination, int count, Action onReceiveDestination = null)
    {
        float delay = _baseStartMoveDelay + _itemStartMoveDelay * count;
        stackItemTransform.DOMove(destination, _moveTime).SetEase(Ease.OutCubic).SetDelay(delay).OnComplete(()=> onReceiveDestination?.Invoke());
        stackItemTransform.DOScale(Vector3.zero, _zoomOutDuration).SetEase(Ease.OutCubic).SetDelay(delay + _moveTime - _zoomOutDuration);
    }


    [NaughtyAttributes.Button("Visualize")]
    private void Visualize()
    {
        IStack stack = _player.Stack.GetStack(ItemType.Wood);
        stack.TrySpend(ItemType.Wood, 10, out IEnumerable<StackItem> items);
        Visualize(items, Vector3.up);
    }
}
