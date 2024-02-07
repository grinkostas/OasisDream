using UnityEngine;

namespace GameCore.Scripts.MysteryBox.Rewards
{
    public abstract class ABoxReward : MonoBehaviour
    {
        public abstract void SpawnReward();
        public abstract void ClaimReward();
    }
}