using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StaserSDK.Interactable;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using Zenject;

public class BuyZoneSaver : Saver<Dictionary<ItemType, int>>
{
    [SerializeField] private ZoneBase _zoneBase;
    [SerializeField] private BuyZone _buyZone;

    private Dictionary<ItemType, int> _defaultValue = new ();

    protected override Dictionary<ItemType, int> DefaultValue
    {
        get
        {
            if (_defaultValue.Count == 0)
                foreach (var type in _buyZone.SourcePrices.Keys)
                    _defaultValue.Add(type, 0);

            return _defaultValue;
        }
    }

    private void OnEnable()
    {
        if (_buyZone.IsBought())
        {
            _buyZone.Buy();
            return;
        }

        _zoneBase.OnExit += OnBuyProgressChanged;
    }

    private void OnDisable()
    {
        _zoneBase.OnExit -= OnBuyProgressChanged;
    }

    protected override Dictionary<ItemType, int> GetSaveData() => _buyZone.UsedResources;
    
    public override Dictionary<ItemType, int> GetSave()
    {
        return ValidSave(base.GetSave());
    }

    private Dictionary<ItemType, int> ValidSave(Dictionary<ItemType, int> save)
    {
        foreach (var sourcePricePair in _buyZone.SourcePrices)
        {
            save.TryAdd(sourcePricePair.Key, 0);

            if (save[sourcePricePair.Key] > sourcePricePair.Value)
                save[sourcePricePair.Key] = sourcePricePair.Value;
        }

        var cachedSave = new Dictionary<ItemType, int>(save);
        foreach (var savePair in cachedSave)
        {
            if (_buyZone.SourcePrices.ContainsKey(savePair.Key) == false)
                save.Remove(savePair.Key);
        }

        return save;
    }
    protected override bool NeedToSave()
    {
        if (_buyZone.IsBought())
            return false;
        return true;
    }


    private void OnBuyProgressChanged(InteractableCharacter character)
    {
        Save();
    }


}
