using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NepixSignals;
using Zenject;

public class ModifiersController
{
    [Inject, UsedImplicitly] public DiContainer Container {get;}
    [Inject, UsedImplicitly] public Player Player {get;}
    
    public List<TimeModifier> CurrentModifiers { get; } = new();
    public TheSignal<TimeModifier> Used { get; } = new();
    public TheSignal<TimeModifier> Ended { get; } = new();

    public void UseModifier(TimeModifier timeModifier, float duration = 0.0f)
    {
        Container.Inject(timeModifier);
        
        CurrentModifiers.Add(timeModifier);
        timeModifier.Removed.On(EndModifier);
        timeModifier.Apply(duration);
        Used.Dispatch(timeModifier);
    }

    public void EndModifier(TimeModifier timeModifier)
    {
        if(CurrentModifiers.Contains(timeModifier) == false)
            return;
        
        timeModifier.Removed.Off(EndModifier);
        CurrentModifiers.Remove(timeModifier);
        Ended.Dispatch(timeModifier);
    }
}
