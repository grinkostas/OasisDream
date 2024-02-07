using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FuelTank : MonoBehaviour
{
    public abstract bool IsEnoughFuel();
    public abstract void UseFuel();
}
