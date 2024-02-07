using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NepixSignals;
using StaserSDK.Interactable;
using Zenject;

public abstract class RewardBooster : MonoBehaviour, IPoolItem<RewardBooster>
{
    [SerializeField] private string _id;
    [SerializeField] private Transform _modelParent;
    [SerializeField] private ZoneBase _zoneBase;
    
    [Inject, UsedImplicitly] public Player Player { get; }
    [Inject, UsedImplicitly] public BoostersController BoostersController { get; }

    public string Id => _id;
    public int TakeTimes => ES3.Load(Id, 0);
    public ZoneBase Zone => _zoneBase;
    public Transform ModelParent => _modelParent;
    public IPool<RewardBooster> Pool { get; set; }
    public bool IsTook { get; set; }
    
    public TheSignal Removed { get; } = new();
    public TheSignal Claimed { get; } = new();

    public void Take()
    {
        OnTake();
        Claimed.Dispatch();
        ES3.Save(Id, ES3.Load(Id, 0) + 1);
        Return();
    }

    public void Return()
    {
        Removed.Dispatch();
        //BoostersController.Return(this);
    }
    
    protected abstract void OnTake();
    
    public void TakeItem()
    {
        transform.localScale = Vector3.one;
    }

    public void ReturnItem()
    {
        transform.localScale = Vector3.one;
    }
}
