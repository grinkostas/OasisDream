using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK;
using StaserSDK.Upgrades;
using UnityEngine.Events;
using Zenject;
using StaserSDK.Utilities;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private UpgradeValue _damage;
    [Header("Animations")]
    [SerializeField] private AnimatorLinker _animatorLinker;
    [SerializeField] private string _useAnimation = "Attack";
    
    private Tween _reloadTimer = null;

    private Animator Animator => _animatorLinker.Animator;
    
    public int Damage => DamageModifier.GetValue(_damage.ValueInt);
    public UpgradeValue DamageUpgrade => _damage;
    public ModifierValue<int, IntModifier> DamageModifier { get; } = new();
    public WeaponType Type => _weaponType;
    
    public bool InUse { get; private set; } = false;

    public UnityAction<Weapon> StartUsing { get; set; }
    public UnityAction<Weapon> EndedUsing { get; set; }

    private void OnEnable()
    {
        Refresh();
    }

    private void OnDisable()
    {
        Refresh();
    }

    private void Refresh()
    {
        InUse = false;
        DOTween.Kill(this);
    }

    public void Use()
    {
        if(InUse || enabled == false || gameObject.activeInHierarchy == false)
            return;
        Animator.SetBool(_useAnimation, true);
        InUse = true;
        StartUsing?.Invoke(this);
        
    }

    public void AbortUse()
    {
        DOTween.Kill(this);
        EndUse();
    }
    
    private void EndUse()
    {
        Refresh();
        Animator.SetBool(_useAnimation, false);
        EndedUsing?.Invoke(this);
    }
}
