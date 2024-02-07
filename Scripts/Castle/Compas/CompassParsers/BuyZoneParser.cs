using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using StaserSDK.Stack;
using Zenject;

public class BuyZoneParser : CompassItemParser
{
    [Inject, UsedImplicitly] public BuyZoneController BuyZoneController { get; }
    [Inject, UsedImplicitly] public Player Player { get; }
    
    public override Result Parse(List<ItemType> items)
    {
        var result = new Result();
        var buyZone = BuyZoneController.GetCurrentAvailableZone();
        
        if (buyZone == null)
            return result;
        
        if(items == null)
            return result;

        foreach (var item in items)
        {
            if (buyZone.Types.Contains(item))
                return Complete(result, buyZone.Zone.transform.position);
        }

        if (buyZone.Types.Contains(ItemType.Diamond) && Player.Stack.SoftCurrencyStack.ItemsCount > 0)
            return Complete(result, buyZone.Zone.transform.position);

        return result;
    }

   
}
