using GameCore.Scripts.MarketLogic;
using GameCore.Scripts.MarketLogic.Customers;
using GameCore.Scripts.Tasks;
using UnityEngine;

namespace GameCore.Scripts.Castle.Tutorial.Tasks
{
    public class MarketTask : ATask
    {
        [SerializeField] private int _sellAmount;
        [SerializeField] private Market _market;

        private int _currentProgress = 0;

        protected override void OnEnable()
        {
            base.OnEnable();
            _market.Added.On(OnMarketAdd);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        private void OnMarketAdd()
        {
            SetProgress(_currentProgress + 1);
        }

        private void SetProgress(int progress)
        {
            int clampedProgress = Mathf.Clamp(progress, 0, _sellAmount);
            if (clampedProgress != _currentProgress)
            {
                _currentProgress = clampedProgress;
                CurrentValueChanged.Dispatch(_currentProgress);
            }

            if (IsFinished())
                Finish();
        }

        public override float GetCurrentProgress() => GetCurrentValue() / GetFinishValue();

        public override float GetFinishValue() => _sellAmount;

        public override float GetCurrentValue() => _currentProgress;

        protected override bool IsFinishedInternal() => _currentProgress >= _sellAmount;

        public override bool IsAvailableInternal() => !IsFinished();
    }
}