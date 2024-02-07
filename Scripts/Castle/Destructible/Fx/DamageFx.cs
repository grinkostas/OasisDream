using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using StaserSDK.Materials;

public class DamageFx : MonoBehaviour
{
    [SerializeField] private Destructible _destructible;
    [SerializeField] private float _zoomMultiplier;
    [SerializeField] private float _zoomTime;
    [SerializeField] private float _zoomOutTime;
    [SerializeField] private List<Transform> _models;
    [SerializeField] private ParticleSystem _particleSystem;
    
    private void OnEnable()
    {
        _destructible.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _destructible.HealthChanged -= OnHealthChanged;
    }
    
    private void OnHealthChanged()
    {
        foreach (var model in _models)
        {
            model.DOScale(Vector3.one * _zoomMultiplier, _zoomTime).OnComplete(() =>
            {
                model.DOScale(Vector3.one, _zoomOutTime).SetEase(Ease.OutBack);
            });
            if(_particleSystem != null)
                _particleSystem.Play();
        }
    }
}
