using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using StaserSDK.Stack;
using Zenject;

public class StackSizePopupTrigger : BoosterPopupTrigger
{
    

    private IStack Stack => Player.Stack.MainStack;
    
    private void OnEnable()
    {
       Stack.AddedItem += OnAddItem;
    }

    private void OnDisable()
    {
        Stack.AddedItem -= OnAddItem;
    }

    private void OnAddItem(StackItemData itemData)
    {
        if (Stack.SizeModifier.Modified)
            return;

        if (Stack.ItemsCount == Stack.MaxSize)
            ApplyAttempt();
        TryTrigger();
    }
    
}
