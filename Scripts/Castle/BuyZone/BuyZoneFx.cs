using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using StaserSDK.Interactable;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using Zenject;

public class BuyZoneFx : MonoBehaviour
{
    [SerializeField] private BuyZone _buyZone;
    [SerializeField] private Transform _costViewsParent;
    [SerializeField] private BuyZoneCostView _costViewPrefab;
    [SerializeField] private float _zoomOutTime = 0.35f;

    [Inject] private DiContainer _container;

    private Dictionary<ItemType, BuyZoneCostView> _costViews = new ();

    
    private void OnEnable()
    {
        foreach (var price in _buyZone.SourcePrices)
        {
            if(_costViews.ContainsKey(price.Key))
                continue;
            var costView = _container.InstantiatePrefab(_costViewPrefab, _costViewsParent)
                .GetComponent<BuyZoneCostView>();
            costView.Init(_buyZone, price.Key);
            _costViews.Add(price.Key, costView);
        }
        
        _buyZone.UsedResource.On(OnResourceUsed);
    }
    
    private void OnDisable()
    {
        _buyZone.UsedResource.Off(OnResourceUsed);
    }


    private void OnResourceUsed(StackItem stackItem, int least)
    {
        BuyZoneCostView costView = _costViews[stackItem.Type];
        costView.Actualize(least);
        if (_buyZone.IsBought())
            _costViewsParent.DOScale(Vector3.zero, _zoomOutTime);

    }

    
}
