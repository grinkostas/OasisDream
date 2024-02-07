using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelperSleepView : MonoBehaviour
{
    [SerializeField] private HelperState _targetState;
    [SerializeField] private GameObject _model;
    
    private void OnEnable()
    {
        OnExit();
        _targetState.Entered.On(OnEnter);
        _targetState.Exited.On(OnExit);
    }

    private void OnEnter()
    {
        _model.SetActive(true);
    }

    private void OnExit()
    {
        _model.SetActive(false);
    }
}
