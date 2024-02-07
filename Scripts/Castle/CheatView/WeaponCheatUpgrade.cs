using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Upgrades;
using UnityEngine.UI;
using Zenject;

public class WeaponCheatUpgrade : MonoBehaviour
{
    [SerializeField] private Button _upgradeButton;

    [Inject] private UpgradesController _upgradesController;
    [Inject] public Player Player { get; }
    private void OnEnable()
    {
        _upgradeButton.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _upgradeButton.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        var weapons = Player.Equipment.WeaponController.Weapons;
        foreach (var weapon in weapons)
        {
            if(weapon.DamageUpgrade.ConstValue)
                continue;
            var model = _upgradesController.GetModel(weapon.DamageUpgrade.Upgrade);
            if (model.CanLevelUp())
                model.LevelUp();
        }
    }
}
