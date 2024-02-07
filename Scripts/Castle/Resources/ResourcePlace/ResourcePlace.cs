using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Scripts;
using JetBrains.Annotations;
using StaserSDK.Interactable;
using StaserSDK.Stack;
using UnityEngine.Events;
using Zenject;

public class ResourcePlace : Destructible
{
    [SerializeField] private GameObject _checkActiveObject;
    [SerializeField] private Transform _model;
    [SerializeField] private ItemType _placeType;
    [SerializeField] private int _capacity;
    [SerializeField] private bool _taskTarget = true;
    [SerializeField] private bool _finishOnDisable = true;
    
   [Inject, UsedImplicitly] public ResourceController ResourceController { get; }
   [Inject, UsedImplicitly] public Island Island { get; }
    
    public bool AvailableToHelp => _checkActiveObject.activeInHierarchy && _taskTarget;
    public GameObject CheckActivityObject => _checkActiveObject;
    public bool Active { get; private set; } = true;
    public bool DebugActive = true;

    public UnityAction Finished { get; set; }
    public int Capacity => _capacity;

    public ItemType Type => _placeType;
    public Transform Model => _model;

    [Inject]
    public void Inject()
    {
        Island.AddPlace(this);
    }

    private void OnEnable()
    {
        Island.AddPlace(this);
    }

    private void OnDisable()
    {
        Island.RemovePlace(this);
        if(_finishOnDisable)
            Finished?.Invoke();
    }
    
    protected override void OnHealthChanged(int health)
    {
        if (health <= 0)
        {
            Active = false;
            DebugActive = false;
            Finished?.Invoke();
        }
        else
        {
            Active = true;
            DebugActive = true;
        }
    }
    
    protected override void OnRespawn()
    {
        Active = true;
        DebugActive = true;
    }
}
