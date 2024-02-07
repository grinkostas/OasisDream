using System;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using NaughtyAttributes;
using StaserSDK.Upgrades;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Water
{
    public class WaterBottleUpgradeFx : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private WaterBottle _waterBottle;
        [SerializeField] private Transform _bottleModelToScale;
        [SerializeField] private SerializableDictionary<int, float> _scaleByLevel;
        [Header("Punch")]
        [SerializeField] private float _punchScale;
        [SerializeField] private float _punchDuration;
        [SerializeField] private Ease _punchEase; 
        [Header("Zoom Out")]
        [SerializeField] private float _zoomOutDuration;
        [SerializeField] private Ease _zoomOutEase;
        
        private void OnEnable()
        {
            _waterBottle.CapacityUpgradeValue.ModelUpgraded.On(OnUpgradeBottle);
            OnUpgradeBottle();
        }

        private void OnDisable()
        {
            _waterBottle.CapacityUpgradeValue.ModelUpgraded.Off(OnUpgradeBottle);
        }

        [Button("Test")]
        private void OnUpgradeBottle()
        {
            DOTween.Kill(this);
            int level = _waterBottle.CapacityUpgradeValue.CurrentLevel;
            float targetScale = _scaleByLevel[level];
            
            var sequence = DOTween.Sequence();
            sequence.Append(_bottleModelToScale.DOScale(_punchScale, _punchDuration).SetEase(_punchEase));
            sequence.Append(_bottleModelToScale.DOScale(targetScale, _zoomOutDuration).SetEase(_zoomOutEase));
            sequence.SetId(this);
            
            _particleSystem.Play();
        }
    }
}