using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using Zenject;

public class BoostersController : MonoBehaviour
{
    [SerializeField] private List<RewardBooster> _rewardBoosters;
    [SerializeField] private Transform _parent;
    [SerializeField] private int _poolSize;
    [Space]
    [SerializeField] private float _particleReturnDelay;
    [SerializeField] private float _boosterHideDelay;

    [Inject, UsedImplicitly] public DiContainer DiContainer { get; }
    [Inject, UsedImplicitly] public ParticlesController ParticlesController { get; }

    private bool _initialized = false;
    
    private Dictionary<Type, SimplePool<RewardBooster>> _boosterPools = new();
    private Dictionary<Type, List<BoosterSpawnPoint>> _boosterPoints = new();
    

    [Inject]
    private void Initialize()
    {
        if(_initialized)
            return;
        
        foreach (var rewardBooster in _rewardBoosters)
        {
            var pool = new SimplePool<RewardBooster>(rewardBooster, _poolSize, _parent);
            pool.Initialize(DiContainer);
            _boosterPools.Add(rewardBooster.GetType(), pool);
        }

        _initialized = true;
    }

    public void AddPoint<TBoosterSpawnPoint>(TBoosterSpawnPoint spawnPoint) where TBoosterSpawnPoint : BoosterSpawnPoint
    {
        Type type = typeof(TBoosterSpawnPoint);
        if (_boosterPoints.ContainsKey(type) == false)
            _boosterPoints[type] = new List<BoosterSpawnPoint>();

        spawnPoint.Disabled.Once(() => RemovePoint(spawnPoint));
        var pointsPool = _boosterPoints[type];
        if (pointsPool.Contains(spawnPoint))
            return;
                
        pointsPool.Add(spawnPoint);
    }
    
    public void RemovePoint<TBoosterSpawnPoint>(TBoosterSpawnPoint spawnPoint) where TBoosterSpawnPoint : BoosterSpawnPoint
    {
        var type = typeof(TBoosterSpawnPoint);
        if (_boosterPoints.ContainsKey(type) == false)
        {
            return;
        }

        _boosterPoints[type].Remove(spawnPoint);
    }
    
    public bool TryGetPoints<TBoosterSpawnPoint>(out List<BoosterSpawnPoint> spawnPoints) where TBoosterSpawnPoint : BoosterSpawnPoint
    {
        spawnPoints = null;
        if (_boosterPoints.ContainsKey(typeof(TBoosterSpawnPoint)) == false)
            return false;
        spawnPoints = _boosterPoints[typeof(TBoosterSpawnPoint)].Where(x=>x.gameObject.activeInHierarchy).ToList();
        if (spawnPoints.Count == 0)
            return false;
        return true;
    }
    
    public TRewardBooster Get<TRewardBooster>() where TRewardBooster : RewardBooster
    {
        Initialize();
        var pool = _boosterPools[typeof(TRewardBooster)];
        return (TRewardBooster)pool.Get();
    }

    public string GetId<TRewardBooster>()
    {
        Initialize();
        var pool = _boosterPools[typeof(TRewardBooster)];
        return pool.Prefab.Id;
    }

    public RewardBooster Get(string id)
    {
        Initialize();
        var pool = GetPool(id);
        return pool.Get();
    }
    
    public void Return(RewardBooster booster)
    {
        Initialize();
        ParticlesController.Create<PoofParticle>(booster.transform.position, _particleReturnDelay);
        DOVirtual.DelayedCall(_boosterHideDelay, ()=>{
            GetPool(booster.Id).Return(booster);
        }).SetId(this);
    }

    private SimplePool<RewardBooster> GetPool(string id)
    {
        foreach (var boosterPool in _boosterPools)
        {
            if (boosterPool.Value.Prefab.Id == id)
                return boosterPool.Value;
        }

        return null;
    }

}
