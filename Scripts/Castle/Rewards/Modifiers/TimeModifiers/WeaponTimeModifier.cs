using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

public class WeaponTimeModifier : TimeModifier
{
    [Inject, UsedImplicitly] public Player Player { get; }
    [Inject, UsedImplicitly] public SkinManager SkinManager { get; }
    
    private Weapon _targetWeapon;
    private IntModifier _modifier;

    private bool _changeSkin = false;
    private WeaponType _weaponType;
    private string _targetItemId;
    private SkinnedItem _targetItem;
    private SkinData _targetSkin;
    public override float DefaultDuration => Settings.WeaponUpgradeDuration;
 
    public WeaponTimeModifier(string id, WeaponType weaponType, IntModifier damageModifier) : base(id)
    {
        _modifier = damageModifier;
        _weaponType = weaponType;
    }

    public override void OnInject()
    {
        base.OnInject();
        _targetWeapon = Player.Equipment.WeaponController.GetWeaponByType(_weaponType);
    }

    public void SetSkin(string itemId, SkinData skinData)
    {
        _changeSkin = true;
        _targetSkin = skinData;
        _targetItemId = itemId;
    }

    
    protected override void OnApplyModifier()
    {
        if(_targetWeapon == null)
            return;
        _targetWeapon.DamageModifier.AddModifier(this, _modifier);
        if (_changeSkin == false)
            return;
        _targetItem = SkinManager.GetItemBuyId(_targetItemId);
        SkinManager.ChangeSkin(_targetItem, _targetSkin.Id);
    }

    protected override void OnRemoveModifier()
    {
        _targetWeapon.DamageModifier.RemoveModifier(this);
        if (_changeSkin == false)
            return;
        _targetItem.EquipSavedSkin();
        _targetItem = null;
    }
}
