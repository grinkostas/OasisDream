using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;
using Zenject;

public class SoftCurrencyDisplay : ResourceView
{
    [Inject] public Player Player { get; }
    
    private void Start()
    {
        Init(Player.Stack.SoftCurrencyStack, ItemType.Diamond);
    }
}
