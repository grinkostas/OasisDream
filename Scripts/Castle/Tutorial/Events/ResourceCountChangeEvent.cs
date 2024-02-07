using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK.Stack;
using UnityEngine.Events;
using Zenject;

public class ResourceCountChangeEvent : MonoBehaviour, ITutorialEvent
{
    [SerializeField] private TutorialStepBase _tutorialStep;
    [SerializeField] private ItemType _targetType;
    [SerializeField] private int _targetCount;
    [SerializeField] private CountChangeType _countChangeType;
    [SerializeField] private bool _playerStack = true;
    [SerializeField] private bool _startListenAtStart;
    [SerializeField, ShowIf(nameof(_startListenAtStart))] private float _visibleTargetCount;
    [SerializeField, HideIf(nameof(_playerStack))] private StackProvider _stackProvider;

    private int _currentDelta = 0;
    private float _progress = 0;
    private IStack Stack => _playerStack ? _player.Stack.MainStack : _stackProvider.Interface;
    internal enum CountChangeType
    {
        Earn, 
        Spend
    }
    
    [Inject] private Player _player;

    public float FinalValue => _startListenAtStart? _visibleTargetCount : _targetCount;
    public float Progress
    {
        get => _progress;
        set
        {
            if (value < _progress)
                return;
            _progress = value;
            ProgressChanged?.Invoke(_progress);
        }
    }
    
    public UnityAction Finished { get; set; }
    public UnityAction Available { get; set; }
    
    public UnityAction<float> ProgressChanged { get; set; }

    private void OnEnable()
    {
        if(_startListenAtStart)
            Stack.TypeCountChanged += OnTypeCountChanged;
        _tutorialStep.Entered += OnStepEntered;
        _tutorialStep.Exited += OnStepExited;
    }

    private void OnDisable()
    {
        if(_startListenAtStart)
            Stack.TypeCountChanged -= OnTypeCountChanged;
        _tutorialStep.Entered -= OnStepEntered;
        _tutorialStep.Exited -= OnStepExited;
    }


    private void OnStepEntered(TutorialStepBase step)
    {
        if (_startListenAtStart == false)
        {
            _currentDelta = Stack.Items[_targetType].Value;
            Stack.TypeCountChanged += OnTypeCountChanged;
            Progress = _currentDelta / (float)_targetCount;
        }
        else
        {
            Progress = _currentDelta / (float)_targetCount;
        }
        
        if (IsFinished())
        {
            Finished?.Invoke();
        }
    }

    private void OnStepExited(TutorialStepBase step)
    {
        Stack.TypeCountChanged -= OnTypeCountChanged;
    }
    

    private void OnTypeCountChanged(ItemType type, int count)
    {
        if(type != _targetType)
            return;
        
        if(count > 0 && _countChangeType == CountChangeType.Spend)
            return;
        
        if(count < 0 && _countChangeType == CountChangeType.Earn)
            return;

        int coefficient = count < 0 ? -1 : 1;
        _currentDelta += coefficient * count;
        Progress = _currentDelta / (float)_targetCount;
        if (IsFinished())
        {
            Finished?.Invoke();
        }
    }

    public bool IsFinished()
    {
        return _currentDelta >= _targetCount;
    }

    public bool IsAvailable()
    {
        return true;
    }
}
