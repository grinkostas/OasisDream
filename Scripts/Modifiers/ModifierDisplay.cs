using System;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MysteryBox.Rewards
{
    public abstract class ModifierDisplay : MonoBehaviour
    {
        [SerializeField] private string _targetModifierId;
        [Inject] public ModifiersController ModifiersController { get; }

        public bool ModifierApplied { get; private set; } = false;
        
        private void OnEnable()
        {
            ModifiersController.Used.On(OnUsedModifier);
            ModifiersController.Ended.On(OnEndedModifier);
        }

        private void OnUsedModifier(TimeModifier modifier)
        {
            if(modifier.Id != _targetModifierId)
                return;
            ModifierApplied = true;
            OnAppliedModifier();
        }
        private void OnEndedModifier(TimeModifier modifier)
        {
            if(modifier.Id != _targetModifierId)
                return;
            ModifierApplied = false;
            OnAppliedDisappear();
        }

        protected abstract void OnAppliedModifier();
        protected abstract void OnAppliedDisappear();
        
    }
}