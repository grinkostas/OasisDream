using System;
using DG.Tweening;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Water
{
    public class WaterGiver : MonoBehaviour
    {
        [SerializeField] private EquippedCharacterZone _equippedCharacterZone;
        [SerializeField] private float _oneUnitFillDuration = 0.33f;

        private bool _startedTimer = false;

        [Inject, UsedImplicitly] public Player Player { get; }
        
        public TheSignal<WaterBottle> StartedReceiving { get; } = new();
        public TheSignal<EquippedCharacter, string> NewMessage { get; } = new();
        public TheSignal<WaterBottle> StoppedReceiving { get; } = new();
        
        private void OnEnable()
        {
            _equippedCharacterZone.OnInteractInternal += OnInteract;
        }

        private void OnDisable()
        {
            _equippedCharacterZone.OnInteractInternal -= OnInteract;
        }

        private void OnInteract(EquippedCharacter character)
        {
            if (character.WaterBottle.IsFull)
            {
                NewMessage.Dispatch(character, "MAX");
                return;
            }

            StartedReceiving.Dispatch(character.WaterBottle);
            Player.Movement.DisableHandle(this);
            var waterBottle = character.WaterBottle;
            float fillDuration = (waterBottle.MaxCapacity - waterBottle.WaterAmount) * _oneUnitFillDuration;
            DOVirtual.Float(waterBottle.WaterAmount, waterBottle.MaxCapacity, fillDuration,
                value => waterBottle.AddWater(Mathf.CeilToInt(value)))
                .OnComplete(() =>
                {
                    NewMessage.Dispatch(character, "MAX");
                    StoppedReceiving.Dispatch(character.WaterBottle);
                    Player.Movement.EnableHandle(this);
                });
        }
    }
}