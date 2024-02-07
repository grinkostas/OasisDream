using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MysteryBox
{
    public class MysteryBoxSpawner : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnPoint;
        [SerializeField] private MysteryBox _mysteryBoxPrefab;
        [SerializeField] private float _rewardMaxDuration;
        [SerializeField] private float _rewardSpawnDelay;

        [Inject] public DiContainer Container { get; }
        
        private string _mysteryBoxLifetimeId = "MysteryBoxLifetimeId";
        
        private void OnEnable()
        {
            DelayedSpawn();
        }

        private void OnDisable()
        {
            DOTween.Kill(this);
        }

        private void DelayedSpawn()
        {
            DOVirtual.DelayedCall(_rewardSpawnDelay, ()=>
            {
                Spawn();
                DelayedSpawn();
            }).SetId(this);
        }

        [Button()]
        private void Spawn()
        {
            var spawnPoint = _spawnPoint.Random();
            MysteryBox box = Container.InstantiatePrefab(_mysteryBoxPrefab).GetComponent<MysteryBox>();
            box.transform.position = spawnPoint.position;

            var callback = box.SpawnedReward.Once(_ => DOTween.Kill(_mysteryBoxLifetimeId));
            DOVirtual.DelayedCall(_rewardMaxDuration, ()=>
            {
                box.Disable();
                callback.Off();
            }).SetId(_mysteryBoxLifetimeId);
        }
        
        
        
        

    }
}