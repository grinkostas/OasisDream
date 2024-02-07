using System;
using NepixSignals;
using StaserSDK.Interactable;
using UnityEngine;

namespace GameCore.Scripts.MysteryBox
{
    public class MysteryBoxTrigger : MonoBehaviour
    {
        [SerializeField] private InteractableCharacterZone _zone;

        public TheSignal Triggered { get; } = new();
        
        private void OnEnable()
        {
            _zone.OnInteract += OnInteract;
        }

        private void OnDisable()
        {
            _zone.OnInteract -= OnInteract;
        }

        private void OnInteract(InteractableCharacter _)
        {
            Triggered.Dispatch();
        }
    }
}