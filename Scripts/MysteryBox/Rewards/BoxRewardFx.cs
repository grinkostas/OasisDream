using System;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MysteryBox.Rewards
{
    public class BoxRewardFx : MonoBehaviour
    {
        [SerializeField] private MysteryBox _mysteryBox;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private MoveFx _moveFx;
        [SerializeField] private StackItemRotator _stackItemRotator;

        [Inject] public Player Player { get; }
        
        private void OnEnable()
        {
            _mysteryBox.SpawnedReward.On(OnSpawnedReward);
        }

        private void OnDisable()
        {
            _mysteryBox.SpawnedReward.Off(OnSpawnedReward);
        }

        private void OnSpawnedReward(ABoxReward reward)
        {
            var rewardTransform = reward.transform;
            rewardTransform.SetParent(null);
            rewardTransform.position = _spawnPoint.position;

            _moveFx.Move(rewardTransform, Player.transform);
            _stackItemRotator.Rotate(rewardTransform, _moveFx.Duration);
        }
    }
}