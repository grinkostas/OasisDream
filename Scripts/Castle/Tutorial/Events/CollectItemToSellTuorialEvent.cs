using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;
using UnityEngine.Events;
using Zenject;

public class CollectItemToSellTuorialEvent : MonoBehaviour, ITutorialEvent
{
    [SerializeField] private TutorialStep _tutorialStep;
    
    [Inject] private Player _player;
    
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
    
    private IStack Stack => _player.Stack.MainStack;
    
    private void OnEnable()
    {
        _tutorialStep.Entered += OnStepEntered;
        _tutorialStep.Exited += OnStepExited;
    }

    private void OnDisable()
    {
        _tutorialStep.Entered -= OnStepEntered;
        _tutorialStep.Exited -= OnStepExited;
    }

    private void OnStepEntered(TutorialStepBase step)
    {
        Progress = 0;
        
        if (Stack.ItemsCount > 0)
        {
            Finish();
            return;
        }

        Stack.TypeCountChanged += OnTypeCountChanged;
    }

    private void OnStepExited(TutorialStepBase step)
    {
        Progress = 0;
        Stack.TypeCountChanged -= OnTypeCountChanged;
    }
    

    private void OnTypeCountChanged(ItemType type, int count)
    {
        if (Stack.ItemsCount > 0)
            Finish();
    }

    private void Finish()
    {
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
