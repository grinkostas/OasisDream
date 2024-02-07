using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

public class SpeedTimeModifier : TimeModifier
{
    [Inject, UsedImplicitly] public Player Player { get; }
    
    private FloatModifier _modifier;
    public override float DefaultDuration => Settings.IncreaseSpeedDuration;

    public SpeedTimeModifier(string id, float increaseValue, NumericAction numericAction = NumericAction.Multiply) : base(id)
    {
        _modifier = new FloatModifier(increaseValue, numericAction);
    }
    
    protected override void OnApplyModifier()
    {
        Player.CharacterControllerMovement.Speed.AddModifier(this, _modifier);
    }

    protected override void OnRemoveModifier()
    {
        Player.CharacterControllerMovement.Speed.RemoveModifier(this);
    }
}
