using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Interactable;
using UnityEngine.Events;
using Zenject;

public class BuyZoneInteractBlocker : InteractCondition
{
    [SerializeField] private string _requireBuyZoneId;

    [Inject] public BuyZoneController BuyZoneController { get; }

    private bool _initialized = false;
    private bool _targetBuyZoneInitialized = false;
    private BuyZone _targetBuyZone;
    
    private bool _blocked = true;

    public UnityAction<BuyZone> Blocked { get; set; }
    
    public override bool CanInteract(InteractableCharacter character)
    {
        Init();
        if (_requireBuyZoneId == "" || _blocked == false || _targetBuyZoneInitialized == false)
            return true;
        Blocked?.Invoke(_targetBuyZone);
        return false;
    }

    private void Init()
    {
        if(_initialized)
            return;
        _targetBuyZoneInitialized = BuyZoneController.TreGetZone(_requireBuyZoneId, out _targetBuyZone);
        _initialized = true;
        if (_requireBuyZoneId == "" ||  _targetBuyZoneInitialized == false)
        {
            _blocked = false;
            return;
        }
        
        if (_targetBuyZone.IsBought())
        {
            _blocked = false;
            return;
        }

        _targetBuyZone.Bought.On(OnBought);
    }

    private void OnBought()
    {
        _targetBuyZone.Bought.Off(OnBought);
        _blocked = false;
    }
}
