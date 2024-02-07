using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NepixSignals;
using Zenject;

public abstract class BoosterSpawnPoint : MonoBehaviour
{
    [SerializeField] private BoosterModel _modelPrefab;
    [Inject, UsedImplicitly] public BoostersController BoostersController { get; }


    private InitializedProperty<BoosterModel> _modelProperty;

    public BoosterModel Model => _modelProperty.Value;
    
    public TheSignal Disabled { get; } = new();

    [Inject]
    public void Construct()
    {
        _modelProperty = new InitializedProperty<BoosterModel>(SpawnModel);
        if(enabled)
            AddPointToController();
    }

    private BoosterModel SpawnModel()
    {
        BoosterModel model = Instantiate(_modelPrefab);
        model.gameObject.SetActive(false);
        return model;
    }
    
    private void OnEnable()
    {
        AddPointToController();
    }

    public void OnDisable()
    {
        RemovePointController();
        Disabled.Dispatch();
    }

    public abstract void AddPointToController();

    public abstract void RemovePointController();
}
