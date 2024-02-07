using GameCore.Scripts.Tasks;
using StaserSDK.Interactable;
using StaserSDK.Stack;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Castle.Tutorial.Tasks
{
    public class CollectResourcesTask : ATask
    {
        [SerializeField] private ItemType _collectType;
        [SerializeField] private int _collectCount;
        [SerializeField] private int _substactAmount = 0;
        [Inject] public Player Player { get; }

        private int _currentProgress = 0;

        private StackBase Stack =>
            _collectType == ItemType.Diamond ? Player.Stack.SoftCurrencyStack : Player.Stack.MainStack;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            Stack.TypeCountChanged += OnTypeCountChanged;
            SetProgress(Stack.Items[_collectType].Value);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Stack.TypeCountChanged -= OnTypeCountChanged;
        }
        
        private void OnTypeCountChanged(ItemType type, int delta)
        {
            if(type != _collectType)
                return;
            if(delta <= 0)
                return;
            SetProgress(_currentProgress+delta);
        }

        private void SetProgress(int progress)
        {
            int clampedProgress = Mathf.Clamp(progress, 0, _collectCount);
            if (clampedProgress != _currentProgress)
            {
                _currentProgress = clampedProgress;
                CurrentValueChanged.Dispatch((int)GetCurrentValue());
            }
            if(IsFinished())
                Finish();
        }

        public override float GetCurrentProgress() => GetCurrentValue() / GetFinishValue();

        public override float GetFinishValue() => _collectCount - _substactAmount;

        public override float GetCurrentValue() => _currentProgress - _substactAmount;

        protected override bool IsFinishedInternal() => _currentProgress >= _collectCount;

        public override bool IsAvailableInternal() => !IsFinished();
    }
}