using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfiniteFuelTank : FuelTank
{
    public override bool IsEnoughFuel()
    {
        return true;
    }

    public override void UseFuel()
    {
    }
}
