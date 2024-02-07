using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using DG.Tweening;
using JetBrains.Annotations;
using Zenject;

public class ModifiersSave
{
    [Inject, UsedImplicitly] public ModifiersController modifiersController { get; }
    
    private const string DurationParameter = "duration";
    private const float SaveDelay = 2.0f;

    [Inject, UsedImplicitly]
    private void Inject()
    {
        modifiersController.Used.On(OnApplyModifer);
        modifiersController.Ended.On(OnRemoveModifier);
        ApplySave();
    }

    private void ApplySave()
    {
        foreach (var modifierPair in Modifiers.Ids)
        {
            if(ES3.KeyExists(modifierPair.Key) == false)
                continue;
            var modifier = modifierPair.Value();
            ApplySavedModifier(modifier);
        }
    }

    private void ApplySavedModifier(TimeModifier timeModifier)
    {
        var modifierData = ES3.Load<ModifierData>(timeModifier.Id);
        timeModifier.SetMaxDuration(modifierData.Duration);
            
        var duration = GetCurrentDuration(timeModifier);
        modifiersController.UseModifier(timeModifier, duration);
    }

    private float GetCurrentDuration(TimeModifier timeModifier)
    {
        return ES3.Load(GetDurationSaveParameter(timeModifier.Id), 0.0f);
    }

    private void OnApplyModifer(TimeModifier timeModifier)
    {
        timeModifier.OnDurationChanged.On(OnDurationChanged);
        ES3.Save(timeModifier.Id, GetModifierData(timeModifier));
    }
    
    private void OnRemoveModifier(TimeModifier timeModifier)
    {
        timeModifier.OnDurationChanged.Off(OnDurationChanged);
        ES3.DeleteKey(GetDurationSaveParameter(timeModifier.Id));
        ES3.DeleteKey(timeModifier.Id);
    }

    private void OnDurationChanged(TimeModifier timeModifier)
    {
        if(DOTween.IsTweening(timeModifier))
            return;
        ES3.Save($"{timeModifier.Id}_{DurationParameter}", timeModifier.Duration);
        DOVirtual.DelayedCall(SaveDelay, () => { }).SetId(timeModifier);
    }
    
    private ModifierData GetModifierData(TimeModifier timeModifier)
    {
        return new ModifierData(timeModifier.Id, DateTime.Now, timeModifier.DefaultDuration);
    }

    private static string GetDurationSaveParameter(string id)
    {
        return $"{id}_{DurationParameter}";
    }


}
