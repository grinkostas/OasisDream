using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;


[System.Serializable]
public class StackSaveData
{
    public int Count;
    public ItemType Type;

    public StackSaveData(ItemType type, int count)
    {
        Type = type;
        Count = count;
    }
}
