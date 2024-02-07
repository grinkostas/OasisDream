using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

public class RecylerSpeedTimeModifier : TimeModifier
{
    [Inject, UsedImplicitly] public RecyclersController RecyclersController { get; }
    
    private string _recyclerId = "Sawmill";
    private Recycler _recycler;
    private FloatModifier _modifier;
    
    public override float DefaultDuration => Settings.RecycleBoostDuration;
    
    public RecylerSpeedTimeModifier(string id, FloatModifier speedModifier) : base(id)
    {
        _modifier = speedModifier;
    }

    public void SetRecycler(string recycleId)
    {
        _recyclerId = recycleId;
        RecyclersController.TryGet(_recyclerId, out _recycler);
    }
    
    protected override void OnApplyModifier()
    {
        _recycler.RecycleTimeModifier.AddModifier(this, _modifier);
    }

    protected override void OnRemoveModifier()
    {
        _recycler.RecycleTimeModifier.RemoveModifier(this);
    }
}
