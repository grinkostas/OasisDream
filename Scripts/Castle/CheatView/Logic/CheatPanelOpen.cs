using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.Scripts.Popups;
using GameCore.Scripts.Popups.PopupVariants;
using JetBrains.Annotations;
using UnityEngine.UI;
using Zenject;

public class CheatPanelOpen : MonoBehaviour
{
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private List<Side> _sidesQueue;
    [SerializeField] private float _refreshDelay;

    [Inject, UsedImplicitly] public PopupFactory PopupFactory { get; }
    
    private List<Side> _currentSidesQueue = new();
    private bool _opened = false;

    [System.Serializable]
    public enum Side
    {
        Left, 
        Right
    }

    [NaughtyAttributes.Button()]
    public void Show()
    {
        PopupFactory.Show<CheatPanelPopup>();;
    }
    
    private void OnEnable()
    {
        ResetQueue();
        _leftButton.onClick.AddListener(OnLeftButtonClick);
        _rightButton.onClick.AddListener(OnRightButtonClick);
    }

    private void OnDisable()
    {
        _leftButton.onClick.RemoveListener(OnLeftButtonClick);
        _rightButton.onClick.RemoveListener(OnRightButtonClick);
    }

    private void OnLeftButtonClick()
    {
        OnButtonClick(Side.Left);
    }

    private void OnRightButtonClick()
    {
        OnButtonClick(Side.Right);
    }

    private void OnButtonClick(Side side)
    {
        if (_opened)
        {
            PopupFactory.Show<CheatPanelPopup>();
            return;
        }
        if(_currentSidesQueue == null || _currentSidesQueue.Count == 0)
            return;

        if (side != _currentSidesQueue[0])
        {
            ResetQueue();
            return;
        }
        
        _currentSidesQueue.Remove(_currentSidesQueue[0]);
        if (_currentSidesQueue.Count == 0)
        {
            ResetQueue();
            PopupFactory.Show<CheatPanelPopup>();
            _opened = true;
            SDKController.SendEvent("Opened_Cheat_Panel");
            return;
        }

        ResetTimer();

    }

    private void ResetTimer()
    {
        DOTween.Kill(this);
        DOVirtual.DelayedCall(_refreshDelay, ResetQueue).SetId(this);
    }

    private void ResetQueue()
    {
        _currentSidesQueue = new(_sidesQueue);
    }
    
    
}
