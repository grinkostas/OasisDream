using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using StaserSDK.Stack;
using Zenject;

public class RecyclersParser : CompassItemParser
{
    [Inject, UsedImplicitly] public RecyclersController RecyclersController { get; }
    
        
    public override Result Parse(List<ItemType> items)
    {
        var result = new Result();
        
        foreach (var item in items)
        {
            if(RecyclersController.TryGetBySourceType(item, out Recycler recycler) == false)
                continue;
            
            if(recycler.gameObject.activeInHierarchy == false)
                continue;
                
            result.Completed = true;
            result.TargetPosition = recycler.Zone.transform.position;
        }

        return result;
    }
}
