using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using NepixSignals;
using StaserSDK.Extentions;
using StaserSDK.Stack;
using StaserSDK.Upgrades;
using UnityEngine.Events;
using Zenject;

public abstract class StackBase : MonoBehaviour, IStack
{
    [SerializeField, HideIf(nameof(_capacityFromUpgrade))] private int _maxSize;
    [SerializeField] private bool _capacityFromUpgrade;
    [SerializeField, ShowIf(nameof(_capacityFromUpgrade))] private UpgradeValue _capacityUpgrade;
    [SerializeField] private bool _storeAnyType;
    [SerializeField, HideIf(nameof(_storeAnyType))] private List<ItemType> _storedTypes;

    [Inject] private UpgradesController _upgradesController;
    private Dictionary<ItemType, IntReference> _items = new();
    private bool _initialized = false;
    private int _count = 0;

    private List<object> _blockers = new List<object>();

    public int ItemsCount => _count;
    public ModifierValue<int, IntModifier> SizeModifier { get; } = new();
    public int MaxSize => _capacityFromUpgrade ? SizeModifier.GetValue(_capacityUpgrade.ValueInt) :  _maxSize;
    public List<StackItem> SourceItems { get; } = new List<StackItem>();

    private bool _infinite = false;
    
    public UpgradeValue CapacityUpgradeValue => _capacityUpgrade;

    public Dictionary<ItemType, IntReference> Items
    {
        get
        {
            if(_initialized == false)
                Initialize();
            return _items;
        }
    }

    public bool Disabled => _blockers.Count > 0;
    
    public UnityAction<StackItemData> AddedItem { get; set; }
    public UnityAction<StackItemData> TookItem { get; set; }
    public UnityAction<ItemType, int> TypeCountChanged { get; set; }
    public UnityAction<int> CountChanged { get; set; }

    public TheSignal OnEnable { get; } = new();
    public TheSignal OnDisable { get; } = new();

    private void Awake()
    {
        Initialize();
    }

    public void MakeInfinite()
    {
        _infinite = true;
        foreach (var item in Items)
        {
            item.Value.MakeInfinite();
        }
    }

    public void Clear()
    {
        var items = new List<StackItem>(SourceItems);

        foreach (var item in items)
        {
            SourceItems.Remove(item);
            Items[item.Type].Value -= item.Amount;
            _count = CalculateCount();
            
            CountChanged?.Invoke(_count);
            TypeCountChanged?.Invoke(item.Type, -item.Amount);
            TookItem?.Invoke(new StackItemData(item).AddDestination(transform));
        }
        
        SourceItems.Clear();
        Items.Clear();
        
        _initialized = false;
        Initialize();
    }
    
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if(_initialized)
            return;
        InitializeItemDictionary(_items, () => new IntReference());
        _initialized = true;
    }

    protected void InitializeItemDictionary<T>(Dictionary<ItemType, T> dictionary, Func<T> getDefaultValue)
    {
        if (_storeAnyType || _storedTypes.Has(x=>x == ItemType.Any))
        {
            foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
            {
                dictionary.Add(itemType, getDefaultValue());
            }
        }
        else
        {
            foreach (ItemType suit in _storedTypes)
            {
                dictionary.Add(suit, getDefaultValue());
            }
        }
    }

    public bool StoreType(ItemType itemType)
    {
        foreach (var storedType in _storedTypes)
        {
            if (storedType == itemType || storedType == ItemType.Any)
                return true;
        }

        return false;
    }

    public bool TryAdd(StackItem stackItem)
    {
        if (gameObject.activeInHierarchy == false)
            return false;
        Initialize();
        if (Disabled)
            return false;
        if (_infinite == false && _count + stackItem.Amount > MaxSize)
            return false;

        Add(stackItem);
        return true;
    }

    public void Add(ItemType itemType, int amount)
    {
        Initialize();
        Items[itemType].Value += amount;
        
        TypeCountChanged?.Invoke(itemType, amount);
        CountChanged?.Invoke(_count);
    }

    public void Add(StackItem stackItem, bool skipAnimation = false)
    {
        Initialize();
        var countRef = Items[stackItem.Type];
        countRef.Value += stackItem.Amount;
        _count = CalculateCount();
        if (skipAnimation == false)
        {
            SourceItems.Add(stackItem);
            var itemData = new StackItemData(stackItem);
            OnAddItem(itemData);
            AddedItem?.Invoke(itemData);
        }
       
        TypeCountChanged?.Invoke(stackItem.Type, stackItem.Amount);
        CountChanged?.Invoke(_count);
    }

    private int CalculateCount()
    {
        return Items.Sum(x => x.Value.Value);
    }
    
    protected virtual void OnAddItem(StackItemData stackItem)
    {
    }

    public bool TryAddRange(ItemType type, int count)
    {
        if (Disabled)
            return false;
        if (gameObject.activeInHierarchy == false)
            return false;
        Initialize();
        if (_count + count > MaxSize)
            return false;

        var countRef = Items[type];
        countRef.Value += count;
        _count = CalculateCount();
        TypeCountChanged?.Invoke(type, count);
        CountChanged?.Invoke(_count);
        return true;
    }

    public bool TryTake(ItemType itemType, out StackItem stackItem, Transform destination, StackItemDataModifier modifier = new StackItemDataModifier())
    {
        stackItem = null;
        if (itemType == ItemType.Any)
        {
            if (TryGetLastType(out itemType) == false)
                return false;
        }
        
        if (gameObject.activeInHierarchy == false)
            return false;

        if (_infinite == false)
        {
            var itemCountRef = Items[itemType];
            if (itemCountRef.Value <= 0)
                return false;
            itemCountRef.Value--;
        }
       
        if (TryGetInstance(itemType, out stackItem) == false)
            return false;

        SourceItems.Remove(stackItem);
        
        OnTakeItem(stackItem);
        TookItem?.Invoke(new StackItemData(stackItem).AddDestination(destination).ApplyModifier(modifier));
        if (_infinite == false)
        {
            _count = CalculateCount();
            TypeCountChanged?.Invoke(itemType, -stackItem.Amount);
            CountChanged?.Invoke(_count);
        }
        
        return true;
    }
    
    protected virtual void OnTakeItem(StackItem stackItem)
    {
    }

    protected abstract bool TryGetInstance(ItemType type, out StackItem stackItem);
    protected abstract IEnumerable<StackItem> GetInstances(ItemType type, int count);

    public bool TryGetLastType(out ItemType itemType)
    {
        itemType = ItemType.None;
        if (SourceItems.Count == 0)
        {
            foreach (var itemsPair in Items)
            {
                if (itemsPair.Value.Value > 0)
                {
                    itemType = itemsPair.Key;
                    break;
                }
            }

            if (itemType == ItemType.None)
                return false;
            return true;
        }
        
        itemType = SourceItems.Last().Type;
        return true;
    }
    
    public bool TryTakeLast(out StackItem stackItem, Transform destination, StackItemDataModifier modifier = new StackItemDataModifier())
    {
        stackItem = null;
        if (gameObject.activeInHierarchy == false)
            return false;
        if (TryGetLastType(out ItemType takeType) == false)
            return false;
        return TryTake(takeType, out stackItem, destination, modifier);
    }

    public bool TrySpend(ItemType type, int amount)
    {
        if (gameObject.activeInHierarchy == false)
            return false;
        
        var countRef = Items[type];
        
        if (countRef.Value < amount)
            return false;
        
        countRef.Value -= amount;

        _count = Items.Sum(x=>x.Value.Value);
        TypeCountChanged?.Invoke(type, -amount);
        CountChanged?.Invoke(_count);
        return true;
    }

    public bool TrySpend(ItemType type, int amount, out IEnumerable<StackItem> spendItems)
    {
        spendItems = default;
        if (gameObject.activeInHierarchy == false)
            return false;
        if (TrySpend(type, amount) == false)
            return false;
        spendItems = GetInstances(type, amount); 
        return true;
    }

    public void DisableCollect(object sender)
    {
        if (_blockers.Contains(sender))
            return;
        _blockers.Add(sender);
        if(_blockers.Count == 1)
            OnDisable.Dispatch();
    }

    public void EnableCollect(object sender)
    {
        if(_blockers.Contains(sender) == false)
            return;
        
        _blockers.Remove(sender);
        if (_blockers.Count == 0)
            OnEnable.Dispatch();
    }
}
