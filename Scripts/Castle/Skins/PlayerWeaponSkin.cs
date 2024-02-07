using JetBrains.Annotations;
using StaserSDK.Upgrades;
using UnityEngine;
using Zenject;

public class PlayerWeaponSkin : SkinnedItem
{
    [SerializeField] private WeaponType _weaponType;
    [Inject, UsedImplicitly] public Player Player { get; }

    public UpgradeValue UpgradeValue => Player.Equipment.WeaponController.GetWeaponByType(_weaponType).DamageUpgrade;
    
    protected override void OnEnableInternal()
    {
        UpgradeValue.ModelUpgraded.On(EquipSavedSkin);
    }
    
    protected override string GetDefaultSkinId()
    {
        if (Skins.Count == 0)
            return DefaultSkin.Id;
        int skinId = Mathf.Min(UpgradeValue.CurrentLevel, Mathf.Max(0, Skins.Count-1));
        return Skins[skinId].Id;
    }
}
