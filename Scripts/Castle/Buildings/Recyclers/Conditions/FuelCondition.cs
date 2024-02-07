using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuelCondition : RecycleCondition
{
    [SerializeField] private FuelTank _fuelTank;

    public override bool CanRecycle()
    {
        return _fuelTank.IsEnoughFuel();
    }

    public override void HandleConditionPassed()
    {
        _fuelTank.UseFuel();
    }
}
