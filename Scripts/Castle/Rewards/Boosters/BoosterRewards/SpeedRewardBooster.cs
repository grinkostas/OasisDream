using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

public class SpeedRewardBooster : LimitedTimeRewardBooster
{
    [SerializeField] private string _modifierId = "DefaultSpeedModifier";

    public override float Duration => Modifiers.GetModifier(_modifierId).MaxDuration;
    
    [Inject, UsedImplicitly] public ModifiersController modifiersController { get; }
    
    protected override void OnTake()
    {
        modifiersController.UseModifier(Modifiers.GetModifier(_modifierId));
    }
}
