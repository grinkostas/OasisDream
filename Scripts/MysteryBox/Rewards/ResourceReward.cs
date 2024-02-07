using DG.Tweening;
using StaserSDK.Stack;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MysteryBox.Rewards
{
    public class ResourceReward : ABoxReward
    {
        [SerializeField] private int _amount;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private float _returnDelay;
        [SerializeField] private Transform _modelWrapper;

        [Inject] public ResourceController ResourceController { get; }
        [Inject] public Player Player { get; }

        public override void SpawnReward()
        {
            var item = ResourceController.GetInstance(_itemType);
            item.Claim();
            item.transform.SetParent(_modelWrapper);
            item.transform.localPosition = Vector3.zero;
            DOVirtual.DelayedCall(_returnDelay, () => item.Pool.Return(item));
        }

        public override void ClaimReward()
        {
            var stack = Player.Stack.GetStack(_itemType);
            stack.Add(_itemType, _amount);
        }
        
    }
}