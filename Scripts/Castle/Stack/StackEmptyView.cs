using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK.Stack;
using Zenject;

public class StackEmptyView : MonoBehaviour
{
    [SerializeField] private View _viewToHide;
    [SerializeField] private bool _playerStack = true;
    [SerializeField, HideIf(nameof(_playerStack))]
    private StackProvider _stack;

    [Inject] private Player _player;

    private IStack Stack => _playerStack ? _player.Stack.MainStack : _stack.Interface;

    private void OnEnable()
    {
        Stack.CountChanged += OnCountChanged;
    }
    
    private void OnDisable()
    {
        Stack.CountChanged -= OnCountChanged;
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
        if(Stack.ItemsCount == 0)
            _viewToHide.Hide();
        else
            _viewToHide.Show();
    }
}
