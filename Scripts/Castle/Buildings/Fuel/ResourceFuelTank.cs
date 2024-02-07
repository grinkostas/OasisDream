using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;

public class ResourceFuelTank : FuelTank
{
    [SerializeField] private StackProvider _fuelStack;
    [SerializeField] private ItemType _fuelType;
    [SerializeField] private int _fuelAmountForOneAction;

    public override bool IsEnoughFuel()
    {
        return _fuelStack.Interface.ItemsCount >= _fuelAmountForOneAction;
    }

    public override void UseFuel()
    {
        if(IsEnoughFuel() == false)
            return;
        _fuelStack.Interface.TrySpend(_fuelType, _fuelAmountForOneAction);
    }
    
}
