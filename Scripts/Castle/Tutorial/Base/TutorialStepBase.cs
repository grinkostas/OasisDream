using System;
using Cinemachine;
using DG.Tweening;
using GameCore.Scripts.Tasks;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Zenject;

public abstract class TutorialStepBase : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private bool _activeSelf;
    [SerializeField, HideIf(nameof(_lastStep))] private TutorialStepBase _nextStep;
    [SerializeField] private ATask _task;
    [SerializeField] private bool _lastStep;
    [SerializeField] private bool _showCamera;
    [SerializeField, ShowIf(nameof(_showCamera))] private float _showCameraDelay;
    [SerializeField] private bool _disableTaskOnFinish = true;

    [Inject] private TutorialPointer _tutorialPointer;

    
    public ATask Task => _task;
    
    public abstract Transform Target { get; }
    public bool IsFinished => ES3.Load(_id, false);
    
    public event UnityAction<TutorialStepBase> Entered;
    public event UnityAction<TutorialStepBase> Exited;
    public event UnityAction<TutorialStepBase> Ended;
    
    
    private void Start()
    {
        _task.gameObject.SetActive(false);
        if(_activeSelf)
            Enter();
    }

    public void Enter()
    {
        if (ES3.Load(_id, false) || _task.IsFinished())
        {
            NextStep();
            return;
        }

        ForceEnter();
    }

    public void ForceEnter()
    {
        if(_task == null)
            return;
        
        if (_showCamera)
        {
            DOVirtual.DelayedCall(_showCameraDelay, _task.ClickInteraction).SetId(_showCameraDelay);
        }
        
        OnEnter();
        _task.Finished.On(NextStep);
        ApplyTarget();

        Entered?.Invoke(this);
        SDKController.LevelStart(_id);
        _task.gameObject.SetActive(true);
    
    }

    protected virtual void OnEnter()
    {
    }
    
    
    private void ApplyTarget()
    {
        _tutorialPointer.SetTarget(Target);
    }

    public void NextStep(ATask task) => NextStep();
    public void NextStep()
    {
        DOTween.Kill(_showCameraDelay);
        _task.Finished.Off(NextStep);
        _task.Finish();
        Exit();
        if (_lastStep == false)
            _nextStep.Enter();
    }
    
    
    public void Exit(bool forced = false)
    {
        if (forced == false)
        {
            ES3.Save(_id, true);
            if (ES3.Load(_id, false) == false)
                SDKController.LevelComplete(_id);
        } 

        OnExit();
        
        DOTween.Kill(this);

        if(forced == false)
            Ended?.Invoke(this);
        
        Exited?.Invoke(this);
        if(_disableTaskOnFinish)
            _task.gameObject.SetActive(false);
        _task.Finished.Off(NextStep);
        _tutorialPointer.ReceiveDestination();
    }

    protected virtual void OnExit()
    {
        
    }

}
