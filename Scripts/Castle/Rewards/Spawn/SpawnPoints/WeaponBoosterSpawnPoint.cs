using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class WeaponBoosterSpawnPoint : BoosterSpawnPoint
{
    [SerializeField] private float _duration;
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private IntModifier _intModifier;
    [SerializeField] private bool _changeSkin;
    [SerializeField, ShowIf(nameof(_changeSkin))] private string _itemId;
    [SerializeField, ShowIf(nameof(_changeSkin))] private SkinData _skin;

    public float Duration => _duration;
    public WeaponType WeaponType => _weaponType;
    public IntModifier IntModifier => _intModifier;

    public bool ChangeSkin => _changeSkin;
    public SkinData Skin => _skin;
    public string ItemForSkinId => _itemId;
    
    public override void AddPointToController()
    {
        BoostersController.AddPoint(this);
    }

    public override void RemovePointController()
    {
        BoostersController.RemovePoint(this);
    }
}
