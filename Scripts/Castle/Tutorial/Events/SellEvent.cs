using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;
using UnityEngine.Events;

public class SellEvent : MonoBehaviour, ITutorialEvent
{
    [SerializeField] private TutorialStepBase _tutorialStep;
    [SerializeField] private ShopSpot _shopSpot;

    private float _progress = 0;
    public float Progress
    {
        get => _progress;
        private set
        {
            _progress = value;
            ProgressChanged?.Invoke(_progress);
        }
    }
    public float FinalValue => 1;

    public UnityAction Finished { get; set; }
    public UnityAction Available { get; set; }
    public UnityAction<float> ProgressChanged { get; set; }

    
    private void OnEnable()
    {
        _tutorialStep.Entered += OnStepEntered;
        _tutorialStep.Exited += OnStepExited;
    }

    private void OnStepEntered(TutorialStepBase step)
    {
        Progress = 0;
        _shopSpot.Sold += OnSold;
    }

    private void OnStepExited(TutorialStepBase step)
    {
        Progress = 0;
        _shopSpot.Sold -= OnSold;
    }

    private void OnSold(StackItem stackItem)
    {
        _shopSpot.Sold -= OnSold;
        Progress = 1;
        Finished?.Invoke();
    }


    public bool IsFinished()
    {
        return Progress > 0;
    }

    public bool IsAvailable()
    {
        return true;
    }
}
