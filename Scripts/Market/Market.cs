using DG.Tweening;
using JetBrains.Annotations;
using NaughtyAttributes;
using NepixSignals;
using StaserSDK.Interactable;
using StaserSDK.SaveProperties.Api;
using StaserSDK.Stack;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MarketLogic
{
    public class Market : MonoBehaviour
    {
        [SerializeField] private string _marketId;
        [SerializeField] private StackableCharacterZone _zone;
        [Header("Storage")]
        [SerializeField] private MarketStack _marketStack;
        [SerializeField] private int _maxCapacity;
        [SerializeField] private ItemType _sellType;
        [Header("Sell")]
        [SerializeField] private int _sellPrice;
        [SerializeField] private ItemType _rewardType = ItemType.Diamond;
        [SerializeField] private StackBase _rewardStack;
        [Header("Sell Delays")]
        [SerializeField] private float _addDelay;
        [SerializeField] private float _oneCoinAdditionalAddDelay;

        [Inject, UsedImplicitly] public ResourceController ResourceController { get; }
        
        public ItemType SellType => _sellType;
        public int Capacity => _maxCapacity;

        private int _delayedAmount = 0;

        public int StoredAmount
        {
            get =>  ES3.Load(_marketId, 0);
            private set => ES3.Save(_marketId, value);
        }

        public TheSignal Added { get; } = new();
        public TheSignal Sold { get; } = new();
        public TheSignal<StackItem> AddedRewardItem { get; } = new();

        private void OnEnable()
        {
            _zone.OnInteractInternal += OnInteract;
        }

        private void OnDisable()
        {
            _zone.OnInteractInternal -= OnInteract;
        }

        private void OnInteract(StackableCharacter character)
        {
            if(StoredAmount + _delayedAmount >= _maxCapacity)
                return;
            var takeWithSuccess = character.MainStack.TryTake(_sellType, out StackItem item, _marketStack.transform,
                new StackItemDataModifier(_marketStack.GetDelta()));
            
            if(takeWithSuccess == false)
                return;
            
            item.Claim();
            _delayedAmount++;
            DOVirtual.DelayedCall(_addDelay, () => Add(item)).SetUpdate(false);
        }

        private void Add(StackItem item)
        {
            _delayedAmount--;
            StoredAmount++;
            item.Pool.Return(item);
            Added.Dispatch();
            _marketStack.Actualize();
        }

        public void Sell(int amount)
        {
            if(amount > StoredAmount)
                return;
            StoredAmount -= amount;
            int price = _sellPrice * amount;
            _marketStack.Actualize();
            AddCoins(price);
            Sold.Dispatch();
        }

        private void AddCoins(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
             
                DOVirtual.DelayedCall(_oneCoinAdditionalAddDelay * i, () =>
                {
                    var rewardItem = ResourceController.GetInstance(_rewardType);
                    rewardItem.transform.SetParent(_rewardStack.gameObject.transform, false);
                    rewardItem.Claim();
                    _rewardStack.Add(rewardItem);
                    AddedRewardItem.Dispatch(rewardItem);
                });
            }
        }

        [Button()]
        private void Sell()
        {
            int price = _sellPrice * 1;
            _marketStack.Actualize();
            AddCoins(price);
            Sold.Dispatch();
        }
    }
}