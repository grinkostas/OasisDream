using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using NaughtyAttributes;
using StaserSDK.Interactable;
using Zenject;

public abstract class BoosterRewardSpawner<TBoosterReward, TBoosterSpawnPoint> : MonoBehaviour 
    where TBoosterSpawnPoint : BoosterSpawnPoint 
    where TBoosterReward : RewardBooster
{
    [SerializeField] private float _changeBoosterDelay;
    
    [Inject, UsedImplicitly] public BoostersController BoostersController { get; }
    
    public TBoosterReward CurrentBooster { get; private set; }
    public TBoosterSpawnPoint CurrentSpawnPoint { get; private set; }

    private void Start()
    {
        SpawnBooster();
    }

    protected virtual bool NeedToSpawn() => true;
    
    [Button()]
    private void SpawnBooster()
    {
        ResetTimer();
        
        if (NeedToSpawn() == false || TryGetNextPoint(out TBoosterSpawnPoint spawnPoint) == false)
        {
            Clear();
            return;
        }

        if(spawnPoint == CurrentSpawnPoint)
            return;
        Clear();
        SpawnBooster(spawnPoint);

    }

    private bool TryGetNextPoint(out TBoosterSpawnPoint spawnPoint)
    {
        spawnPoint = null;
        if (BoostersController.TryGetPoints<TBoosterSpawnPoint>(out List<BoosterSpawnPoint> points) == false)
            return false;
        spawnPoint = (TBoosterSpawnPoint)points.Random();
        return true;
    }
    
    private void SpawnBooster(TBoosterSpawnPoint spawnPoint)
    {
        CurrentSpawnPoint = spawnPoint;
        CurrentBooster = BoostersController.Get<TBoosterReward>();     
        
        CurrentSpawnPoint.Model.Init(CurrentBooster);
        CurrentSpawnPoint.Disabled.Once(SpawnBooster);
        
        CurrentBooster.Zone.OnInteract += OnInteract;
        CurrentBooster.Zone.OnExit += OnExit;

        CurrentBooster.transform.position = CurrentSpawnPoint.transform.position;

        OnSpawnBooster(spawnPoint);
    }

    protected virtual void OnSpawnBooster(TBoosterSpawnPoint spawnPoint)
    {
    }

    private void Clear()
    {
        if (CurrentBooster == null)
            return;
        CurrentBooster.Removed.Off(SpawnBooster);
        CurrentBooster.Zone.OnInteract -= OnInteract;
        CurrentBooster.Zone.OnExit -= OnExit;
        CurrentBooster.Return();
        CurrentSpawnPoint.Disabled.Off(SpawnBooster);
        CurrentBooster = null;
        CurrentSpawnPoint = null;
        
    }
    
    private void OnInteract(InteractableCharacter character)
    {
        DOTween.Kill(this);
    }

    private void OnExit(InteractableCharacter character)
    {
        ResetTimer();
    }

    private void ResetTimer()
    {
        DOTween.Kill(this);
        DOVirtual.DelayedCall(_changeBoosterDelay, SpawnBooster).SetId(this);
    }
}
