using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK.Stack;
using Zenject;

public class StackMaxView : MonoBehaviour
{
    [SerializeField] private View _maxView;
    [SerializeField] private bool _playerStack = true;

    [SerializeField, HideIf(nameof(_playerStack))]
    private StackProvider _stack;

    [Inject] private Player _player;

    private IStack Stack => _playerStack ? _player.Stack.MainStack : _stack.Interface;

    private void OnEnable()
    {
        Stack.CountChanged += OnCountChanged;
        Stack.SizeModifier.Changed.On(Actualize);
        Actualize();
    }
    
    private void OnDisable()
    {
        Stack.CountChanged -= OnCountChanged;
        Stack.SizeModifier.Changed.Off(Actualize);
    }

    private void Start()
    {
        Actualize();
    }

    private void OnCountChanged(int count)
    {
        Actualize();
    }

    private void Actualize()
    {
        if(Stack.ItemsCount >= Stack.MaxSize)
            _maxView.Show();
        else
            _maxView.Hide();
    }
}
