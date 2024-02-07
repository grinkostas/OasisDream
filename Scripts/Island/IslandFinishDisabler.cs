using System.Collections.Generic;
using GameCore.Scripts.Untils;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts
{
    public class IslandFinishDisabler : MonoBehaviour
    {
        [SerializeField] private EnableOnTriggerEnter _enableOnTriggerEnter;
        [Inject] public Island Island { get; }
        
        private void Start()
        {
            if (Island.IsFinished)
                _enableOnTriggerEnter.Trigger();
        }
    }
}