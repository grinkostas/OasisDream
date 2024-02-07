using System;
using GameCore.Scripts.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Castle.Tutorial.Tasks
{
    public class FillBottleTask : ATask
    {
        [SerializeField] private int _targetFillCount;
        
        [Inject, UsedImplicitly] public Player Player { get; }

        private int _currentProgress = 0;

        protected override void OnEnable()
        {
            base.OnEnable();
            _currentProgress = Player.WaterBottle.WaterAmount;
            WaterCheck(_currentProgress);
            Player.WaterBottle.AddedWater.On(OnAddWater);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Player.WaterBottle.AddedWater.Off(OnAddWater);
        }

        private void OnAddWater()
        {
            WaterCheck(_currentProgress+1);
        }

        private void WaterCheck(int waterAmount)
        {
            int progress = Mathf.Clamp(waterAmount, 0, _targetFillCount);
            if (progress != _currentProgress)
            {
                _currentProgress = progress;
                CurrentValueChanged.Dispatch(waterAmount);
            }

            if (IsFinished())
            {
                Player.WaterBottle.AddedWater.Off(OnAddWater);
                Finish();
            }
        }
        
        public override float GetCurrentProgress() => _currentProgress / (float)_targetFillCount;
        public override float GetFinishValue() => _targetFillCount;
        public override float GetCurrentValue() => _currentProgress;
        protected override bool IsFinishedInternal() => _currentProgress >= _targetFillCount;
        public override bool IsAvailableInternal() => !IsFinished();
    }
}