using NepixSignals;
using StaserSDK.Stack;
using UnityEngine;


public class BuyZoneEvents : MonoBehaviour, IBuyZoneEvents
{
    [SerializeField] private BuyZone _buyZone;

    public BuyZone Zone => _buyZone;

    public bool IsBought() => _buyZone.IsBought();
    public TheSignal Bought => _buyZone.Bought;
    public TheSignal<StackItem, int> UsedResource => _buyZone.UsedResource;
    public TheSignal<float> BuyProgressChangedDelayed => _buyZone.BuyProgressChangedDelayed;
}
