using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using Zenject;

public abstract class RecycleAdditionalFx : MonoBehaviour
{
    [SerializeField] private Recycler _recycler;
    [SerializeField] protected float CycleDuration;
    [SerializeField] protected Transform StartSpawnPoint;
    
    [Inject] public DiContainer Container { get; }
    [Inject] public ResourceController ResourceController { get; }
    [Inject] public Timer Timer { get; }
    
    protected Transform SourceItem;
    protected Transform ProductionItem;
    
    private bool _recycling = false;

    public Recycler Recycler => _recycler;
    
    
    private void OnEnable()
    {
        SourceItem = SpawnResource(_recycler.SourceType);
        ProductionItem = SpawnResource(_recycler.ProductType);
        _recycler.OnStartRecycle += StartRecycle;
        _recycler.OnEndRecycle += EndRecycle;
        OnEnableInternal();
    }
    
    private void OnDisable()
    {
        _recycler.OnStartRecycle -= StartRecycle;
        _recycler.OnEndRecycle -= EndRecycle;
        OnDisableInternal();
    }

    protected virtual void OnEnableInternal(){}
    protected virtual void OnDisableInternal(){}
    
    private Transform SpawnResource(ItemType type)
    {
        var resource =  Container.InstantiatePrefab(ResourceController.GetPrefab(type), StartSpawnPoint).GetComponent<Resource>();
        resource.Claim();
        resource.gameObject.SetActive(false);
        return resource.transform;
    }

    [Button()]
    private void StartRecycle()
    {
        if(_recycling)
            return;
        _recycling = true;
        OnStartRecycle();
        RecycleCycle();
    }

    protected virtual void OnStartRecycle(){}
    private void RecycleCycle()
    {
        if(_recycling == false)
            return;
        DOTween.Kill(this);
        ProductCycle();
        DOVirtual.DelayedCall(CycleDuration, RecycleCycle).SetId(this);
    }

    protected abstract void ProductCycle();

    private void EndRecycle()
    {
        if(_recycling == false)
            return;
        _recycling = false;
        OnEndRecycle();
    }
    protected virtual void OnEndRecycle(){}
}
