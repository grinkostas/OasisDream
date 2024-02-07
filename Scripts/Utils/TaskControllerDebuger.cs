using GameCore.Scripts.Tasks;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Untils
{
    public class TaskControllerDebuger : MonoBehaviour
    {
        [Inject] public TaskController TaskController { get; }

        [Button()]
        private void GetTasks()
        {
            foreach (var task in TaskController._tasks)
            {
                Debug.Log($"{task.Description}, {task.Priority}, {task.IsAvailable()}");
            }
        }
    }
}