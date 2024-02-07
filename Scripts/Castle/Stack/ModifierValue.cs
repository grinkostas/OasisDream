using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NepixSignals;

public class ModifierValue<TValue, TModifier> where TModifier : ValueModifier<TValue>
{
    private List<Tuple<object, TModifier>> _modifiers = new();

    public bool Modified => _modifiers.Count > 0;
    public TheSignal Changed { get; } = new();
    public TValue GetValue(TValue startValue)
    {
        foreach (var modifier in _modifiers) 
            startValue = modifier.Item2.ApplyModifier(startValue);

        return startValue;
    }

    public void AddModifier(object sender, TModifier modifier)
    {
        if(_modifiers.Has(x=>x.Item1 == sender))
            return;
        _modifiers.Add(new Tuple<object, TModifier>(sender, modifier));
        Changed.Dispatch();
    }

    public void RemoveModifier(object sender)
    {
        var modifier = _modifiers.Find(x=> x.Item1 == sender);
        if(modifier == null)
            return;
        _modifiers.Remove(modifier);
        Changed.Dispatch();
    }
}
