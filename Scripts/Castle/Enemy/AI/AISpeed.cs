using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AISpeed
{
    private float _startSpeed;
    private List<SpeedModifier> _modifiers = new ();
    
    public float Speed { get; private set; }

    public AISpeed(float startSpeed)
    {
        _startSpeed = startSpeed;
        Speed = startSpeed;
    }

    public void AddModifier(SpeedModifier modifier)
    {
        var existedModifier = _modifiers.Find(x => x.Sender == modifier.Sender);
        if (existedModifier != null)
        {
            if(existedModifier.Modifier == modifier.Modifier)
                return;
            _modifiers.Remove(existedModifier);
        }
        _modifiers.Add(modifier);
        ActualizeModifiers();
    }

    public void RemoveModifier(object sender)
    {
        var modifier = _modifiers.Find(x => x.Sender == sender);
        if (modifier == null)
            return;
        
        _modifiers.Remove(modifier);
        ActualizeModifiers();
    }

    private void ActualizeModifiers()
    {
        float modifiedSpeed = _startSpeed;
        
        foreach (var modifier in _modifiers)
        {
            modifiedSpeed = modifier.CalculateSpeed(modifiedSpeed);
        }

        Speed = modifiedSpeed;
    }
}