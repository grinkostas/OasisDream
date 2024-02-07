using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MysteryBox.Rewards
{
    public class MushroomModifier : TimeModifier
    {
        [Inject, UsedImplicitly] public Player Player { get; }
        
        private List<TimeModifier> _modifiers = new();

        public override float DefaultDuration { get; } = 30;
        
        
        public MushroomModifier(string id, DiContainer container, int weaponUpgradeValue = 2) : base(id)
        {
            _modifiers.Add(new SpeedTimeModifier(id, 1.25f, NumericAction.Multiply));
            _modifiers.Add(new WeaponTimeModifier(id, WeaponType.Axe, new IntModifier(weaponUpgradeValue, NumericAction.Add)));
            _modifiers.Add(new WeaponTimeModifier(id, WeaponType.Pickaxe, new IntModifier(weaponUpgradeValue, NumericAction.Add)));
            _modifiers.Add(new WeaponTimeModifier(id, WeaponType.Sword, new IntModifier(weaponUpgradeValue, NumericAction.Add)));
            foreach (var modifier in _modifiers)
            {
                container.Inject(modifier);
                modifier.SetMaxDuration(30);
            }

        }

        
        protected override void OnApplyModifier()
        { 
            foreach (var modifier in _modifiers)
            {
                modifier.Apply();
            }
        }

        protected override void OnRemoveModifier()
        {
            foreach (var modifier in _modifiers)
            {
                modifier.Remove();
            }
        }
    }
}