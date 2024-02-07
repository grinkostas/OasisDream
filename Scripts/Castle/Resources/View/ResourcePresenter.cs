using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using StaserSDK.Extentions;
using StaserSDK.Stack;
using UnityEngine.UI;
using Zenject;

public class ResourcePresenter : MonoBehaviour
{
    [SerializeField] private ResourceView _resourceViewPrefab;
    [SerializeField] private int _maxVisibleViewsCount;
    [SerializeField] private Transform _viewsParent;
    [SerializeField] private StackProvider _stackProvider;
    [SerializeField] private bool _changeOrder = true;
    [SerializeField] private int _additionalOrder;
    [SerializeField] private bool _rebuildParent;

    [SerializeField, ShowIf(nameof(_rebuildParent))]
    private RectTransform _parentToRebuild;

    [Inject] private ResourceController _resourceController;
    [Inject] private DiContainer _container;
    [Inject] private Player _player;

    private bool _inited = false;
    private List<ResourceView> _visibleResourceViews;
    private Dictionary<ItemType, IntReference> Items => _player.Stack.MainStack.Items;

    public List<ResourceView> ResourceViews  {get; private set;} = new ();
    private List<ResourceView> VisibleResourceViews
    {
        get
        {
            if (_visibleResourceViews == null)
                _visibleResourceViews = new List<ResourceView>();
            return _visibleResourceViews;
        }
    }
    
    private void OnEnable()
    {
        if (_changeOrder == false)
            return;
        
        _player.Stack.MainStack.TypeCountChanged += OnTypeCountChanged;
    }

    private void OnDisable()
    {
        if (_changeOrder == false)
            return;
        
        _player.Stack.MainStack.TypeCountChanged -= OnTypeCountChanged;
    }

    private void OnTypeCountChanged(ItemType type, int count)
    {
        if(_rebuildParent)
            LayoutRebuilder.ForceRebuildLayoutImmediate(_parentToRebuild);
        
        Init();
        
        var resourceView = ResourceViews.Find(x=>x.ItemType == type);

        if (VisibleResourceViews.Count > 0 && VisibleResourceViews[0] == resourceView)
        {
            ActualizeViews();
            return;
        }

        VisibleResourceViews.Remove(resourceView);
        VisibleResourceViews.Insert(0, resourceView);
        
        ActualizeViews();
    }
    


    private void ActualizeViews()
    {
        foreach (var resourceView in ResourceViews)
        {
            resourceView.gameObject.SetActive(false);
        }

        _viewsParent.gameObject.SetActive(true);
        for (int i = 0; i < _maxVisibleViewsCount; i++)
        {
            if (VisibleResourceViews[i] != null && VisibleResourceViews[i].CurrentAmount > 0)
            {
                VisibleResourceViews[i].Rect.SetSiblingIndex(i + _additionalOrder);
                VisibleResourceViews[i].gameObject.SetActive(true);
            }
        }
    }
    
    private void Start()
    {
        Init();
    }
    
    private void Init()
    {
        if(_inited)
            return;
        
        foreach (var resource in _resourceController.Resources)
        {
            var resourceView = _container.InstantiatePrefab(_resourceViewPrefab, _viewsParent).GetComponent<ResourceView>();
            resourceView.Init(_stackProvider.Interface, resource.ItemType);
            ResourceViews.Add(resourceView);
            VisibleResourceViews.Add(resourceView);
        }

        _inited = true;
        ActualizeViews();
    }
    
    
}
