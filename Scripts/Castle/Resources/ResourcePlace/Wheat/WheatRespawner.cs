using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK;

public class WheatRespawner : MonoBehaviour
{
    [SerializeField] private Destructible _destructible;
    [SerializeField] private float _oneHpRegenerationTime;

    private void OnEnable()
    {
        _destructible.Damaged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _destructible.Damaged -= OnHealthChanged;
    }

    private void OnHealthChanged(int damage)
    {
        if (_destructible.Health > 0)
            return;

        DOVirtual.DelayedCall(_oneHpRegenerationTime, Heal).SetId(this);
    }

    private void Heal()
    {
        DOTween.Kill(this);
        _destructible.Heal(1);
        if (_destructible.Health == _destructible.MaxHealth)
        {
            _destructible.Respawn();
            return;
        }
        DOVirtual.DelayedCall(_oneHpRegenerationTime, Heal).SetId(this);
    }
    
}
