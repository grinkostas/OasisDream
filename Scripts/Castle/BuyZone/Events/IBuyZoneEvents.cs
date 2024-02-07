using NepixSignals;
using StaserSDK.Stack;
using UnityEngine;
using UnityEngine.Events;


public interface IBuyZoneEvents
{
    public TheSignal Bought { get; }
    public TheSignal<StackItem, int> UsedResource { get; }
    
    
    public TheSignal<float> BuyProgressChangedDelayed { get; }
}
