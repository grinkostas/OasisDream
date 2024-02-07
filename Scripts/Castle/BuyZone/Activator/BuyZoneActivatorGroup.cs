using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using NepixSignals;
using Unity.VisualScripting;
using UnityEngine;

public class BuyZoneActivatorGroup : ABuyZoneActivator
{
    [SerializeField, HideIf(nameof(_getFromChildren))] private List<ABuyZoneActivator> _activators;
    [SerializeField] private bool _getFromChildren;

    private List<ABuyZoneActivator> _activatorsCached;
    private List<ABuyZoneActivator> Activator
    {
        get
        {
            if (_activatorsCached != null)
                return _activatorsCached;
            if (_getFromChildren)
                _activatorsCached = GetComponentsInChildren<ABuyZoneActivator>().ToList();
            else
                _activatorsCached = _activators;
            return _activatorsCached;
        }
    }

    public override TheSignal Bought { get; } = new();
        
    public override bool Has(BuyZone zone)
    {
        return Activator.Has(x => x != null && x.Has(zone));
    }

    private void OnEnable()
    {
        foreach (var activator in Activator)
            activator.Bought.On(Actualize);
    }
    
    private void OnDisable()
    {
        foreach (var activator in Activator)
            activator.Bought.Off(Actualize);
    }

    private void Actualize()
    {
        if(IsBought())
            Bought.Dispatch();
            
    }

    public override void Enable()
    {
        foreach (var activator in Activator)
        {
            if(activator.IsBought())
                continue;
            activator.Enable();
            return;
        }
    }

    public override void EnableAll()
    {
        foreach (var activator in Activator)
        {
            activator.EnableAll();
        }
    }

    public override void DisableAll(bool disableZone = false)
    {
        foreach (var activator in Activator)
            activator.DisableAll(true);
    }

    public override bool IsBought()
    {
        return Activator.TrueForAll(x => x.IsBought());
    }
}
