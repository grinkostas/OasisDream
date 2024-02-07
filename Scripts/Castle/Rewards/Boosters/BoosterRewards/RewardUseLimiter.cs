using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace GameCore.Scripts.Rewards.Boosters.BoosterRewards
{
    public class RewardUseLimiter : MonoBehaviour
    {
        [SerializeField] private int _useLimit = 1;
        [SerializeField] private string _id;
        
        private RewardBooster _rewardBoosterCached; 
        public RewardBooster RewardBooster
        {
            get
            {
                if (_rewardBoosterCached == null)
                    _rewardBoosterCached = GetComponentInChildren<RewardBooster>();
                return _rewardBoosterCached;
            }
        }

        private int UsedCount => ES3.Load(_id, 0);

        [Inject]
        public void OnInject()
        {
            RewardBooster.gameObject.SetActive(false);
        }
        
        private void OnEnable()
        {
            if (UsedCount >= _useLimit)
            {
                RewardBooster.gameObject.SetActive(false);
                return;
            }
            RewardBooster.gameObject.SetActive(true);
            RewardBooster.Claimed.On(OnClaimed);
        }

        private void OnDisable()
        {
            RewardBooster.Claimed.Off(OnClaimed);
        }
        

        private void OnClaimed()
        {
            var currentUseCount = UsedCount + 1;
            ES3.Save(_id, currentUseCount);
        }
    }
}