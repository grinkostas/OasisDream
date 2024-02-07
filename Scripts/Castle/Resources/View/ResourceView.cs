using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class ResourceView : MonoBehaviour, IPoolItem<ResourceView>
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Bouncer _bouncer;
    [SerializeField] private View _zoomView;
    [SerializeField] private bool _selfActualize = true;
    [SerializeField] private string _additionalText = "";
    [SerializeField] private bool _hideOnZero = true;

    [Inject] private ResourceController _resourceController;

    private IStack _stack;
    
    public ItemType ItemType { get; private set; }
    public Image IconImage => _iconImage;
    public TMP_Text AmountText => _amountText;
    public RectTransform Rect => _rect;
    public IPool<ResourceView> Pool { get; set; }
    public bool IsTook { get; set; }
    
    public int CurrentAmount { get; private set; }
    
    public void Init(IStack stack, ItemType itemType)
    {
        _stack = stack;
        BaseInit(itemType);
        Actualize(itemType, _stack.Items[itemType].Value);
        if(_selfActualize)
            _stack.TypeCountChanged += Actualize;
    }

    public ResourceView Init(ItemType itemType, int count)
    {
        BaseInit(itemType);
        Actualize(itemType, count);
        return this;
    }

    private void BaseInit(ItemType itemType)
    {
        ItemType = itemType;
        _iconImage.sprite = _resourceController.GetPrefab(ItemType).Icon;
    }
    
    public void Actualize(ItemType itemType, int count)
    {
        if(itemType != ItemType)
            return;
        
        if(_bouncer != null)
            _bouncer.Bounce();
        
        int itemsCount;
        if (_stack != null)
            itemsCount = _stack.Items[itemType].Value;
        else
            itemsCount = count;
        
        CurrentAmount = itemsCount;
        
        if(itemsCount <= 0 && _hideOnZero)
            _zoomView.Hide();
        else
            _zoomView.Show();
        
        _amountText.text = _additionalText + itemsCount;
    }

    public void TakeItem()
    {
        _zoomView.Show();
    }

    public void ReturnItem()
    {
        _zoomView.Hide();
        if(_stack != null)
            _stack.TypeCountChanged -= Actualize;
    }

    public void Hide() => _zoomView.Hide();
}
