using System.Collections.Generic;
using DG.Tweening;
using GameCore.Scripts.MysteryBox.Rewards;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MysteryBox
{
    public class MysteryBox : MonoBehaviour
    {
        [SerializeField] private MysteryBoxTrigger _mysteryBoxTrigger;
        [SerializeField] private float _disableHandleDuration;

        [Header("Rewards")] 
        [SerializeField] private List<ABoxReward> _rewards;
        [SerializeField] private float _spawnDelay;
        [SerializeField] private float _claimDelay;
        [SerializeField] private float _destroySelfDelay;

        [Inject] public Player Player { get; }

        public TheSignal<ABoxReward> SpawnedReward { get; } = new();
        public TheSignal ClaimedReward { get; } = new();
        public TheSignal Disabled { get; } = new();

        private void OnEnable()
        {
            _mysteryBoxTrigger.Triggered.On(OnTriggered);
        }

        private void OnDisable()
        {
            _mysteryBoxTrigger.Triggered.Off(OnTriggered);
        }

        private void OnTriggered()
        {
            _mysteryBoxTrigger.gameObject.SetActive(false);
            DisableHandle();
            CreateReward();
        }

        private void DisableHandle()
        {
            Player.Movement.DisableHandle(this);
            DOVirtual.DelayedCall(_disableHandleDuration, ()=> Player.Movement.EnableHandle(this)).SetId(this);
        }
        
        private void CreateReward()
        {
            var reward = _rewards.Random();
            DOVirtual.DelayedCall(_spawnDelay, ()=>
            {
                reward.SpawnReward();
                SpawnedReward.Dispatch(reward);
            });
            DOVirtual.DelayedCall(_spawnDelay+_claimDelay, ()=>
            {
                ClaimedReward.Dispatch();
                Disable();
                reward.ClaimReward();
            });
        }

        public void Disable()
        {
            _mysteryBoxTrigger.gameObject.SetActive(false);
            Disabled.Dispatch();
            DOVirtual.DelayedCall(_destroySelfDelay, ()=>
            {
                Destroy(gameObject);
            });
        }
    }
}