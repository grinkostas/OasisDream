using System;
using Haptic;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Castle.Fx
{
    public class DestructibleVibration : MonoBehaviour
    {
        [SerializeField] private Destructible _destructible;

        [Inject] public IHapticService HapticService { get; }
        
        private void OnEnable()
        {
            _destructible.HealthChanged += Vibrate;
        }

        private void OnDisable()
        {
            _destructible.HealthChanged -= Vibrate;
        }

        private void Vibrate()
        {
            HapticService.Selection();
        }

        
    }
}