using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using StaserSDK.Stack;
using Zenject;

public class RecycleForBuyParser : CompassItemParser
{
    [Inject, UsedImplicitly] public Player Player { get; }
    [Inject, UsedImplicitly] public BuyZoneController BuyZoneController { get; }
    [Inject, UsedImplicitly] public RecyclersController RecyclersController { get; }
    
        
    public override Result Parse(List<ItemType> items)
    {
        var result = new Result();
        var buyZone = BuyZoneController.GetCurrentAvailableZone();

        foreach (var item in items)
        {
            if(RecyclersController.TryGetBySourceType(item, out Recycler recycler) == false)
                continue;
            
            if(recycler.gameObject.activeInHierarchy == false)
                continue;

            if(buyZone.Types.Contains(recycler.ProductType))
                return Complete(result, recycler.Zone.transform.position);
        }

        if (Player.Stack.MainStack.ItemsCount >= Player.Stack.MainStack.MaxSize)
            return result;

        if (buyZone == null)
            return result;
        
        foreach (var type in buyZone.Types)
        {
            if(type is ItemType.Any or ItemType.Gold or ItemType.None or ItemType.Diamond)
                continue;

            if (RecyclersController.TryGetByProductType(type, out Recycler recycler) == false)
                continue;
            
            if(recycler.gameObject.activeInHierarchy == false)
                continue;
            
            if (recycler.ProductionStack.Interface.ItemsCount > 0)
                return Complete(result, recycler.ProductionStack.GameObject.transform.position);
        }
        return result;
    }
    
    
}
