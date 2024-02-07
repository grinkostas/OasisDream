using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;

namespace GameCore.Scripts.Tasks
{
    public class TaskController
    {
        public List<ITask> _tasks = new();

        public TheSignal<ITask> AddedTask { get; } = new();
        public TheSignal<ITask> TaskFinished { get; } = new();

        public void AddTask(ITask task)
        {
            if(_tasks.Contains(task))
                return;
            if(task.IsFinished())
                return;
            
            _tasks.Add(task);
            task.Finished.Once(RemoveTask);
            AddedTask.Dispatch(task);
        }

        public void RemoveTask(ITask task)
        {
            task?.Finished.Off(RemoveTask);
            _tasks.Remove(task);
            TaskFinished.Dispatch(task);
        }
        public bool TryGetTask(out ITask task)
        {
            task = _tasks.FindAll(x =>x.IsAvailable() && x.IsFinished() == false)
                .OrderByDescending(x=>x.Priority).FirstOrDefault();
            return task != null;
        }
    }
}