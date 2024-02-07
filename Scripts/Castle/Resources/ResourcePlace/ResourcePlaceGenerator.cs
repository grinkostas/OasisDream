using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;
using UnityEngine.Events;
using Zenject;

public class ResourcePlaceGenerator : ResourceGenerator
{
    [SerializeField] private ResourcePlace _resourcePlace;

    [Inject] private ResourceController _resourceController;
    
    public Sprite ResourceIcon => _resourceController.GetPrefab(_resourcePlace.Type).Icon;

    private float DamageToGenerate => _resourcePlace.MaxHealth / (float)_resourcePlace.Capacity;
    private float _damaged = 0.0f;
    
    public UnityAction<StackItem> Spawned { get; set; }
    public override Transform Parent => transform;

    private void OnEnable()
    {
        _resourcePlace.Damaged += OnDamaged;
    }
    
    private void OnDisable()
    {
        _resourcePlace.Damaged -= OnDamaged;
    }

    private void OnDamaged(int damage)
    {
        _damaged += damage;
        if(_damaged < DamageToGenerate)
            return;
        
        int collectedAmount = (int)(_damaged/DamageToGenerate);
        _damaged -= collectedAmount * DamageToGenerate;

        for (int i = 0; i < collectedAmount; i++)
            SpawnResource(_resourcePlace.Type);

    }
    
}
