using System;
using Cinemachine;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Tasks
{ 
    public interface ITask
    {
        public int Priority { get; }
        public Sprite Icon { get; }
        public string Description { get; }
        public int CurrentValue { get; }
        public int FinishValue { get; }
        public float Progress { get; }

        public TheSignal<ATask> Finished { get; }
        public TheSignal<int> CurrentValueChanged { get; } 
        public float GetCurrentProgress();
        public float GetFinishValue();
        public float GetCurrentValue();
        public bool IsFinished();
        public bool IsAvailable();
        public void ClickInteraction();
    }
    public abstract class ATask : MonoBehaviour, ITask
    {
        [SerializeField] private int _priority;
        [SerializeField] private CinemachineVirtualCamera _taskCamera;
        [SerializeField] protected Sprite _icon;
        [SerializeField] protected string _description;
        [SerializeField] private bool _addTaskWhenInject = true;
        [Inject, UsedImplicitly] public TaskController TaskController { get; }
        
        public Sprite Icon => _icon;
        public string Description => _description;
        public int Priority => _priority;

        public int CurrentValue => Mathf.CeilToInt(GetCurrentValue());
        public int FinishValue => Mathf.CeilToInt(GetFinishValue());
        public float Progress => GetCurrentProgress();

        private bool _ended = false;
        
        public TheSignal<ATask> Finished { get; } = new();
        public TheSignal<int> CurrentValueChanged { get; } = new();

        [Inject]
        public void Inject()
        {
            OnInject();
            if(_addTaskWhenInject)
                TaskController.AddTask(this);
        }
        protected virtual void OnInject(){}
        
        private void Awake()
        {
            if(_taskCamera != null)
                _taskCamera.gameObject.SetActive(false);
        }
        protected virtual void OnEnable()
        {
            if(_addTaskWhenInject == false)
                TaskController.AddTask(this);
        }
        
        protected virtual void OnDisable()
        {
            if(_addTaskWhenInject == false)
                TaskController.RemoveTask(this);
        }

        public abstract float GetCurrentProgress();
        public abstract float GetFinishValue();
        public abstract float GetCurrentValue();


        public bool IsFinished() => _ended || IsFinishedInternal();
        protected abstract bool IsFinishedInternal();

        public bool IsAvailable() => gameObject.activeInHierarchy && IsAvailableInternal();
        public abstract bool IsAvailableInternal();

        public virtual void ClickInteraction()
        {
            if(_taskCamera  != null)
                _taskCamera.gameObject.SetActive(true);
        }
        public void Finish()
        {
            _ended = true;
            TaskController.RemoveTask(this);
            CurrentValueChanged.Dispatch(FinishValue);
            Finished.Dispatch(this);
        }
    }

    
}
