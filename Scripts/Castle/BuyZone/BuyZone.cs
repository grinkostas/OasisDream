using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using NepixSignals;
using StaserSDK.Interactable;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using UnityEngine.Analytics;
using UnityEngine.Events;
using Zenject;


public class BuyZone : StackTaker, ITutorialEvent, IBuyZoneEvents
{
    [SerializeField] private List<CostData> _pricesData;
    [SerializeField] private string _buyPropertyName;
    [SerializeField] private BuyZoneSaver _zoneSaver;
    [SerializeField] private float _buyDelay;
    [SerializeField] private float _resourceDeliverTime;
    [SerializeField] private float _targetBuyTime;
    [SerializeField] private bool _takeByOne;

    [Inject] private Timer _timer;
    [Inject] private Player _player;
    [Inject] private BuyZoneController _buyZoneController;
    
    private Dictionary<ItemType, int> _sourcePrices = new();

    private ItemType _lastTakeType;
    private bool _isBought = false;
    private bool _needToTake => !IsBoughtCheck();

    private int _oneInteractTakeCount = 0;

    public string Id => _zoneSaver.Id;
    public string PropertyName => _buyPropertyName;
    public override float Progress
    {
        get
        {
            float progress = 1 - (((float)Prices.Sum(x => x.Value) / _pricesData.Sum(x => x.Amount)));
            return progress;
        }
    }

    public float FinalValue => 1;
    
    protected override int OneTakeCount
    {
        get
        {
            if (_takeByOne)
            {
                _oneInteractTakeCount = 1;
                return 1;
            }
            if (_oneInteractTakeCount == 0)
            {
                int interactsCount = (int)(_targetBuyTime / ZoneBase.InteractTime);
                int pricesSum = _pricesData.Sum(x => x.Amount);
                _oneInteractTakeCount = Mathf.Clamp(Mathf.RoundToInt(pricesSum / (float)interactsCount), 1, int.MaxValue);
            }

            if (_lastTakeType == ItemType.None)
                return 1;
            int playerAmount = _player.Stack.MainStack.Items[_lastTakeType].Value;
            if (_lastTakeType == ItemType.Diamond)
                playerAmount = _player.Stack.SoftCurrencyStack.Items[_lastTakeType].Value;
            
            int highClamp = Mathf.Min(Prices[_lastTakeType], playerAmount);
            
            return Mathf.Clamp(_oneInteractTakeCount, 1, highClamp);
        }
    }

    private List<ItemType> _types = new();
    public List<ItemType> Types
    {
        get
        {
            if (_types.Count == 0)
                _types = SourcePrices.Select(x => x.Key).ToList();
            return _types;
        }
    }

    private Dictionary<ItemType, int> _currentPrices = null;
    public Dictionary<ItemType, int> Prices {
        get
        {
            if (_currentPrices == null)
            {
                _currentPrices = new();
                foreach (var sourcePrice in SourcePrices)
                {
                    _currentPrices.Add(sourcePrice.Key, sourcePrice.Value - UsedResources[sourcePrice.Key]);
                }
            }
            
            return _currentPrices;
        }
    }

    private Dictionary<ItemType, int> _usedResources;
    public Dictionary<ItemType, int> UsedResources
    {
        get
        {
            if (_usedResources == null)
            {
                _usedResources = _zoneSaver.GetSave();
            }

            return _usedResources;
        }
    }
    
    
    public Dictionary<ItemType, int> SourcePrices
    {
        get
        {
            if (!_sourcePrices.Any())
            {
                foreach (var costData in _pricesData)
                {
                    _sourcePrices.Add(costData.Resource, costData.Amount);
                }
            }

            return _sourcePrices;
        }
    }
    
    public UnityAction Finished { get; set; }
    public UnityAction Available { get; set; }
    public UnityAction<float> ProgressChanged { get; set; }
    public TheSignal<int> AbsoluteProgressChanged { get; } = new();
    
    public TheSignal<float> BuyProgressChangedDelayed { get; } = new();
    
    public TheSignal<StackItem, int> UsedResource { get; } = new();
    public TheSignal Bought { get; } = new();
    

    public override ItemType GetTypeToTake(StackableCharacter interactableCharacter)
    {
        foreach (var price in Prices)
        {
            IStack stack = interactableCharacter.GetStack(price.Key);
            
            if (price.Value > 0 && stack.Items[price.Key].Value > 0)
            {
                _lastTakeType = price.Key;
                return price.Key;
            }
        }

        _lastTakeType = Prices.First(x => x.Value > 0).Key;
        return _lastTakeType;
    }

    protected override void OnEnableInternal()
    {
        Available?.Invoke();
        if (Prices.Has(x => x.Value > 0) == false)
            Buy();
    }


    [Inject]
    public void Construct()
    {
        _buyZoneController.AddZone(this);
    }
    
    
    public override bool TakePredicate()
    {
        return _isBought == false & _needToTake;
    }

    protected override void OnTakeItem(StackItem stackItem)
    {
        UsedResources[stackItem.Type] += stackItem.Amount;
        int count = Mathf.Max(0, SourcePrices[stackItem.Type] - UsedResources[stackItem.Type]);

        Prices[stackItem.Type] = count;
        
        if (count <= 0)
            Types.Remove(stackItem.Type);
        
        bool bought = count <= 0 && Prices.Has(x=> x.Value > 0) == false;
        if (bought)
        {
            _isBought = true;
        }

        int absoluteCount = UsedResources.Sum(x => x.Value);
        float currentProgress = Progress;
        DOVirtual.DelayedCall(_resourceDeliverTime, () =>
        {
            stackItem.Pool.Return(stackItem);
            
            ProgressChanged?.Invoke(Progress);
            UsedResource.Dispatch(stackItem, count);
            BuyProgressChangedDelayed.Dispatch(currentProgress);
            AbsoluteProgressChanged.Dispatch(absoluteCount);
            
            if (bought)
            {
                DOVirtual.DelayedCall(_buyDelay, () =>
                {
                    Bought.Dispatch();
                    Finished?.Invoke();
                });
                SDKController.SendEvent(_zoneSaver.Id);
                Debug.Log($"Bought {_zoneSaver.Id}");
            }
        });
    }

    public void Buy()
    {
        if(_isBought)
            return;
        _isBought = true;
        _timer.ExecuteWithDelay(() => Bought.Dispatch(), _buyDelay);
    }

    [Button]
    public void ForceBuy()
    {
        var keys = Prices.Keys.ToList();
        foreach (var key in keys)
        {
            Prices[key] = SourcePrices[key];
        }
        Buy();
    }

    public bool IsBoughtCheck()
    {
        return Prices.Has(x => x.Value > 0) == false;
    }
    
    public bool IsBought()
    {
        return IsBoughtCheck();
    }
    
    public bool IsFinished()
    {
        return IsBoughtCheck();
    }

    public bool IsAvailable()
    {
        return gameObject.activeSelf;
    }
}
