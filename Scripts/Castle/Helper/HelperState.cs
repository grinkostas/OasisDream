using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NepixSignals;
using Zenject;

public abstract class HelperState : MonoBehaviour
{
    [SerializeField] private Helper _helper;
    [SerializeField] private bool _enterOnEnable;
    [SerializeField] private HelperState _nextState;
    
    public Helper Helper => _helper;

    public TheSignal Entered { get; } = new();
    public TheSignal Exited { get; } = new();

    private void Start()
    {
        if (_enterOnEnable == false)
            enabled = false;
        else
            Enter();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void Enter()
    {
        enabled = true;
        OnEnter();
        Entered.Dispatch();
    }

    protected abstract void OnEnter();

    public void Exit()
    {
        if (enabled == false)
            return;
        enabled = false;
        OnExit();
        Exited.Dispatch();
        _nextState.Enter();
    }
    
    protected abstract void OnExit();
}
