using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;
using UnityEngine.Events;
using Zenject;

public class SpendResourceEvent : MonoBehaviour
{
    [SerializeField] private ItemType _targetType;
    [SerializeField] private int _targetCount;
    
    [Inject] private Player _player;

    public UnityAction Finished { get; set; }
    public UnityAction Available { get; set; }

    private void OnEnable()
    {
        _player.Stack.MainStack.AddedItem += OnAddedItem;
        _player.Stack.MainStack.CountChanged += OnCountChanged;
    }

    private void OnDisable()
    {
        _player.Stack.MainStack.AddedItem -= OnAddedItem;
    }

    private void OnCountChanged(int count)
    {
        Actualize();
    }
    
    private void OnAddedItem(StackItemData data)
    {
        Actualize();
    }

    private void Actualize()
    {
        if (IsFinished())
        {
            _player.Stack.MainStack.AddedItem -= OnAddedItem;
            Finished?.Invoke();
        }
    }

    public bool IsFinished()
    {
        return _player.Stack.MainStack.Items[_targetType].Value >= _targetCount;
    }

    public bool IsAvailable()
    {
        return true;
    }
}
