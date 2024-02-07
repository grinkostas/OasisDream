using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using NepixSignals;
using StaserSDK;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Movement _movement;
    [SerializeField] private bool _allowMultipleTarget = true;

    private bool _enabled = true;
    private Weapon _currentWeaponInUse;

    private Destructible _currentTarget;
    private List<Destructible> _availableDestructibles = new();

    private List<object> _blockers = new();
    public bool Disabled => _blockers.Count > 0;

    public List<Weapon> Weapons => _weapons;

    public Weapon GetWeaponByType(WeaponType type) => _weapons.Find(x => x.Type == type);

    public bool InUse => _currentTarget != null || _availableDestructibles.Count > 0;

    public TheSignal StartedUsage { get; } = new();
    public TheSignal EndedUsage { get; } = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Destructible destructible))
        {
            Add(destructible);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Destructible destructible))
        {
            Remove(destructible);
        }
    }

    private void Add(Destructible destructible)
    {
        if(destructible.Health <= 0)
            return;
        if (_availableDestructibles.Contains(destructible))
            return;
        _availableDestructibles.Add(destructible);
        if (_movement != null && _movement.IsMoving)
        {
            _movement.Handler.OnStopMove += Actualize;
            return;
        }

        if (_currentTarget == null || (_currentTarget != null && _currentTarget.Priority <= destructible.Priority))
        {
            _currentTarget = destructible;
            UseWeapon(GetWeaponByType(_currentTarget.TargetWeaponType));
        }
    }
    
    
    private void Remove(Destructible destructible)
    {
        _availableDestructibles.Remove(destructible);
        if(_availableDestructibles.Count == 0)
            AbortUseCurrentWeapon();
        
        if (destructible == _currentTarget)
        {
            _currentTarget = null;
            Actualize();
        }
    }

    private void Actualize()
    {
        if(Disabled)
            return;
        
        if(_movement != null)
            _movement.Handler.OnStopMove -= Actualize;
        
        if (_availableDestructibles.Count == 0)
        {
            AbortUseCurrentWeapon();
            EndedUsage.Dispatch();
            return;
        }
        
        var targetDestructible = _availableDestructibles.OrderBy(x=>x.Priority).First();
        
        if(_currentTarget != null && _currentTarget.Priority >= targetDestructible.Priority)
            return;
        
        UseWeapon(GetWeaponByType(targetDestructible.TargetWeaponType));
        
    }

    private void AbortUseCurrentWeapon()
    {
        if (_currentWeaponInUse != null)
            _currentWeaponInUse.AbortUse();

        _currentWeaponInUse = null;
    }

    private void UseWeapon(Weapon weapon)
    {
        if(Disabled)
            return;
        if(weapon == _currentWeaponInUse)
            return;
        AbortUseCurrentWeapon();
        _currentWeaponInUse = weapon;
        _currentWeaponInUse.Use();
    }

    public void AttackStarted()
    {
        StartedUsage.Dispatch();
    }
    
    public void OnWeaponUsed()
    {
        if(_currentWeaponInUse == null)
            return;
        
        var affectedPlaces = _availableDestructibles.FindAll(x => x.TargetWeaponType == _currentWeaponInUse.Type);
        if (_allowMultipleTarget == false)
        {
            var affectedPlace = affectedPlaces.OrderBy(x => VectorExtentions.SqrDistance(transform, x.transform))
                .FirstOrDefault();
            ApplyDamage(affectedPlace);
        }
        else
        {
            foreach (var affectedPlace in affectedPlaces) 
                ApplyDamage(affectedPlace);
        }
        

        if (_currentTarget != null && _currentTarget.Health == 0)
            _currentTarget = null;
        
        Actualize();
    }

    private void ApplyDamage(Destructible destructible)
    {
        destructible.ApplyDamage(_currentWeaponInUse.Damage);
        if(destructible.Health <= 0)
            Remove(destructible);
    }

    public void Disable(object sender)
    {
        AbortUseCurrentWeapon();
        if(_blockers.Contains(sender))
            return;
        _blockers.Add(sender);
        
    }

    public void Enable(object sender)
    {
        _blockers.Remove(sender);
        if(_blockers.Count == 0)
            Actualize();
    }
}
