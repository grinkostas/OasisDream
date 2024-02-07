using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Haptic;
using Zenject;

public class AttackFx : MonoBehaviour
{
    [SerializeField] private Destructible _targetHealth;
    [SerializeField] private View _damageView;
    [SerializeField] private CameraShake _cameraShake;

    [Inject] private IHapticService _hapticService;
    
    private void OnEnable()
    {
        _targetHealth.Damaged += OnDamaged;
    }
    private void OnDisable()
    {
        _targetHealth.Damaged -= OnDamaged;
    }
    
    private void OnDamaged(int damage)
    {
        _cameraShake.Shake();
        //_damageView.Show();
        _hapticService.Selection();
    }
}
