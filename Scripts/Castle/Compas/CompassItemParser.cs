using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;

public abstract class CompassItemParser
{
    public class Result
    {
        public bool Completed { get; set; }
        public Vector3 TargetPosition { get; set; }
    }
    
    public int Priority { get; private set; }
    public abstract Result Parse(List<ItemType> items);

    public CompassItemParser SetPriority(int priority)
    {
        Priority = priority;
        return this;
    }
    
    public Result Complete(Result currentResult, Vector3 targetPosition)
    {
        currentResult.Completed = true;
        currentResult.TargetPosition = targetPosition;
        return currentResult;
    }
    
    
}
