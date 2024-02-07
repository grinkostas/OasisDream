using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Scripts.Water;
using StaserSDK.Interactable;

public class EquippedCharacter : InteractableCharacter
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private WaterBottle _waterBottle;
    public WeaponController WeaponController => _weaponController;
    public WaterBottle WaterBottle => _waterBottle;
    
    private void OnDisable()
    {
        foreach (var weapon in _weaponController.Weapons)     
        {
            weapon.AbortUse();
        }
    }
}
