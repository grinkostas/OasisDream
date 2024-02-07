using System;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using NaughtyAttributes;
using NepixSignals.Api;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Tasks
{
    public class TaskBar : MonoBehaviour
    {
        [SerializeField] private TaskBarView _taskBarView;
        [SerializeField] private float _hideDelay = 2;
        [SerializeField] private float _returnDelay = 4;
        [SerializeField] private float _updateDelay = 1.0f;
        [Inject, UsedImplicitly] public TaskController TaskController { get; }
        [Inject, UsedImplicitly] public Player Player { get; }

        private ITask _currentTask;

        [Inject]
        public void OnInject()
        {
            TaskController.TaskFinished.On(OnTaskFinished);
            TaskController.AddedTask.On(OnAddedTask);
        }
        
        private void Start()
        {
            if(_currentTask == null || _currentTask.IsFinished() || _currentTask.IsAvailable() == false)
                Reload();
        }

        private void OnAddedTask(ITask task)
        {
            if(_currentTask == null || (_currentTask != null && task.Priority > _currentTask.Priority))
                SetTask(task);
        }
        
        private void OnTaskFinished(ITask task)
        {
            if(task != _currentTask && _currentTask != null && _currentTask.IsFinished() == false && _currentTask.IsAvailable())
                return;
            
            CompleteCurrentTask();
            DOVirtual.DelayedCall(_updateDelay, Reload).SetId(_updateDelay);
        }
        
        private void Reload()
        {
            DOTween.Kill(_updateDelay);
            if (TaskController.TryGetTask(out ITask nextTask))
            {
                SetTask(nextTask);
                return;
            }
            DOVirtual.DelayedCall(_updateDelay, Reload).SetId(_updateDelay);
        }

        private void SetTask(ITask task)
        {
            if(task.IsAvailable() == false || task.IsFinished())
                return;
            
            _currentTask = task;
            DOTween.Kill(_returnDelay);
            DOVirtual.DelayedCall(_hideDelay+_returnDelay, ()=> 
            {
                _taskBarView.InitTaskView(task);
                _taskBarView.Show();
            }).SetId(_returnDelay);
        }

        
        private void CompleteCurrentTask()
        {
            if(_currentTask != null && DOTween.IsTweening(_currentTask))
                return;
            _taskBarView.Complete();
            DOVirtual.DelayedCall(_hideDelay, _taskBarView.Hide).SetId(_currentTask);
        }
        
        public void ShowCamera()
        {
            if(_currentTask == null || _currentTask.IsFinished())
                return;
            _currentTask.ClickInteraction();
        }

    }
}