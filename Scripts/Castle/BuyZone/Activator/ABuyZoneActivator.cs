using NepixSignals;
using UnityEngine;


public abstract class ABuyZoneActivator : MonoBehaviour
{
    public abstract void Enable();
    public abstract void EnableAll();
    public abstract void DisableAll(bool disableZone = false);

    public abstract TheSignal Bought { get; }
    public abstract bool IsBought();
    public abstract bool Has(BuyZone zone);
}
