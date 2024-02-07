using System;
using System.Collections.Generic;
using DG.Tweening;
using NepixSignals;
using NepixSignals.Api;
using UnityEngine;
using UnityEngine.Serialization;


public class BuyZonesController : MonoBehaviour
{
    [SerializeField] private List<BuyZoneData> _buyZonesData;
    [SerializeField] private bool _actualize = false;
    [SerializeField] private bool _checkActive = false;

    public TheSignal<ABuyZoneActivator> ActivatedZone { get; } = new();
    
    [System.Serializable]
    public class BuyZoneData
    {
        public ABuyZoneActivator Activator;
        public List<ABuyZoneActivator> NextZones;
        public float ActivateDelay;
    }
    
    private List<ISignalCallback> _subscribeCallbacks = new();

    public bool IsZoneAvailable(BuyZone buyZone)
    {
        var zoneData = _buyZonesData.Find(x =>
            x.NextZones.Has(activator => activator.Has(buyZone) && IsActivatorAvailable(activator) ));
        if (zoneData == null)
            return true;
        if (zoneData.Activator == null)
            return true;
        return zoneData.Activator.IsBought();
    }

    private bool IsActivatorAvailable(ABuyZoneActivator activator)
    {
        return activator != null && activator.IsBought() == false;
    }
    
    private void Awake()
    {
        if (_actualize)
            Actualize();
    }

    private void OnEnable()
    {
        Subscribe();
    }
    
    private void OnDisable()
    {
        foreach (var callback in _subscribeCallbacks)
            callback.Off();
        _subscribeCallbacks.Clear();
    }

    private void Actualize()
    {
        List<BuyZoneData> buyZonesToDisable = new List<BuyZoneData>();
        foreach (var buyZoneData in _buyZonesData)
        {
            if (buyZoneData.Activator.IsBought()) 
                continue;
            buyZonesToDisable.Add(buyZoneData);
            
        }

        foreach (var buyZoneData in buyZonesToDisable)
        {
            buyZoneData.Activator.DisableAll();
            buyZoneData.NextZones.ForEach(x=>
            {
                x.DisableAll(true);
            });
        }
    }
    
    private void Subscribe()
    {
        foreach (var buyZoneData in _buyZonesData)
        {
            var callback = buyZoneData.Activator.Bought.On(() => OnBought(buyZoneData.Activator));
            _subscribeCallbacks.Add(callback);
        }
    }

    private void OnBought(ABuyZoneActivator activator)
    {
        var zoneData = _buyZonesData.Find(x => x.Activator == activator);
        if(zoneData == null)
            return;

        DOVirtual.DelayedCall(zoneData.ActivateDelay, () =>
        {
            zoneData.NextZones.ForEach(x =>
            {
                x.Enable();
                ActivatedZone.Dispatch(x);
            });
        });
    }
    
}
