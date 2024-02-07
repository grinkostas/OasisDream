using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using StaserSDK.Stack;
using Zenject;

public class StackSizeTimeModifier : TimeModifier
{
    [Inject, UsedImplicitly] public Player Player { get; }
    
    private IntModifier _modifier;
    private IStack Stack => Player.Stack.MainStack;
    
    public override float DefaultDuration => Settings.MaxStackDuration;
    
    public StackSizeTimeModifier(string id, IntModifier intModifier) : base(id)
    {
        _modifier = intModifier;
    }

    protected override void OnApplyModifier()
    {
        Stack.SizeModifier.AddModifier(this, _modifier);
    }

    protected override void OnRemoveModifier()
    {
        Stack.SizeModifier.RemoveModifier(this);
    }
}
