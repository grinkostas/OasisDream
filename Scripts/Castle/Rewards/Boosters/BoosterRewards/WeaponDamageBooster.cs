using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

public class WeaponDamageBooster : LimitedTimeRewardBooster
{
    [SerializeField] private string _modifierId = "";
    [Inject, UsedImplicitly] public ModifiersController ModifiersController { get; }

    public WeaponType _targetWeaponType;
    public IntModifier _damageModifier;
    public WeaponTimeModifier _weaponTimeModifier;

    private float _duration = 120f;
    
    public override float Duration => _duration;

    public void Init(WeaponType weaponType, IntModifier modifier, float duration)
    {
        _targetWeaponType = weaponType;
        _damageModifier = modifier;
        _duration = duration;
        _weaponTimeModifier = new WeaponTimeModifier(_modifierId, _targetWeaponType, _damageModifier);
        _weaponTimeModifier.SetMaxDuration(_duration);
    }

    public void SetSkin(string itemId, SkinData skinData)
    {
        _weaponTimeModifier.SetSkin(itemId, skinData);
    }
    
    protected override void OnTake()
    {
        ModifiersController.UseModifier(_weaponTimeModifier);
    }

}
