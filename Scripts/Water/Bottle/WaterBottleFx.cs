using System;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace GameCore.Scripts.Water
{
    public class WaterBottleFx : MonoBehaviour
    {
        [SerializeField] private WaterBottle _waterBottle;
        [SerializeField] private float _minHeight;
        [SerializeField] private float _maxHeight;
        [SerializeField] private float _actualizeDuration;
        [Header("Particle")] 
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private float _waterFxDuration;
        [Header("Pipe")] 
        [SerializeField] private float _disableDelay;
        [SerializeField] private string _holdPipeAnimation;
        
        [Inject, UsedImplicitly] public Player Player { get; }

        private float OneUnitHeight => _maxHeight / _waterBottle.MaxCapacity;

        private WaterBottleModel WaterBottleModel => _waterBottle.WaterBottleModel;
        
        private void OnEnable()
        {
            DisablePipe();
            Actualize(_waterBottle.WaterAmount);
            _waterBottle.WaterAmountChanged.On(Actualize);
            _waterBottle.StartedUsage.On(OnStartedWatering);
            _waterBottle.EndedUsage.On(OnEndedWatering);
        }

        private void OnStartedWatering()
        {
            DOTween.Kill(this);
            _particleSystem.Play();
            _waterBottle.WaterBottleModel.Hose.UseWater();
            EnablePipe();
        }

        private void OnEndedWatering()
        {
            _waterBottle.WaterBottleModel.Hose.ResetBlendShape();
            DOVirtual.DelayedCall(_waterFxDuration, _particleSystem.Stop).SetId(this);
            DOVirtual.DelayedCall(_disableDelay, DisablePipe).SetId(this);
        }

        private void EnablePipe()
        {
            WaterBottleModel.WaterGunWithHose.gameObject.SetActive(true);
            Player.Animator.SetBool(_holdPipeAnimation, true);
        }
        
        private void DisablePipe()
        {
            WaterBottleModel.WaterGunWithHose.gameObject.SetActive(false);
            Player.Animator.SetBool(_holdPipeAnimation, false);
        }

        private void Actualize(int amount)
        {
            DOTween.Kill(WaterBottleModel);
            float yScale = Mathf.Max(_minHeight, amount * OneUnitHeight);
            WaterBottleModel.Water.DOScaleY(yScale, _actualizeDuration).SetId(WaterBottleModel);
            if (amount <= 0)
            {
                DisablePipe();
                _particleSystem.Stop();
            }
        }
    }
}