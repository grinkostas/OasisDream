using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StaserSDK.Stack;
using Zenject;

public class BagPresenter : MonoBehaviour
{
    [SerializeField] private View _bagView;
    [SerializeField] private int _resourcesToShow;

    [Inject] private Player _player;

    private IStack Stack => _player.Stack.MainStack;

    private void OnEnable()
    {
        Stack.CountChanged += OnCountChanged;
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
        int uniqueResources = 0;
        foreach (var pair in Stack.Items)
        {
            if(pair.Value.Value <= 0)
                continue;
            uniqueResources++;
            if (_resourcesToShow <= uniqueResources)
            {
                _bagView.Show();
                return;
            }
        }
        _bagView.Hide();
    }
}
