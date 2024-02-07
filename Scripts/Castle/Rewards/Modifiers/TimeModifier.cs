using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using NepixSignals;
using Zenject;

public abstract class TimeModifier
{
    private float _duration = 0.0f;
    private float _maxDuration;
    
    private bool _isMaxDurationChanged;
    private bool _isApplied = false;
    private bool _isInjected = false;

    public string Id { get; }
    public abstract float DefaultDuration { get; }

    public float Duration
    {
        get => _duration;
        set
        {
            _duration = value;
            OnDurationChanged.Dispatch(this);
        }
    }

    public float MaxDuration => _isMaxDurationChanged ? _maxDuration : DefaultDuration;

    public ModifiersSettings Settings => ModifiersSettings.Value;

    public TheSignal<TimeModifier> Applied { get; } = new();
    public TheSignal<TimeModifier> Removed { get; } = new();
    public TheSignal<TimeModifier> OnDurationChanged { get; } = new();

    public TimeModifier(string id)
    {
        Id = id;
    }

    [Inject]
    public virtual void OnInject()
    {
        _isInjected = true;
    }

    public TimeModifier SetMaxDuration(float duration)
    {
        if (_duration < 0)
            return this;
        _maxDuration = duration;
        _isMaxDurationChanged = true;
        
        return this;
    }

    public void Apply()
    {
        Apply(0);
    }
    
    public void Apply(float currentDuration)
    {
        if(_isApplied || _isInjected == false)
            return;
        
        _duration = currentDuration;
        DOTween.Kill(this);
        DOVirtual.Float(_duration, MaxDuration, MaxDuration-_duration, value => Duration = value)
            .SetId(this)
            .OnComplete(Remove);
        OnApplyModifier();
        
        Applied.Dispatch(this);
    }

    protected abstract void OnApplyModifier();

    public void Remove()
    {
        _duration = 0.0f;
        DOTween.Kill(this);
        Removed.Dispatch(this);
        OnRemoveModifier();
    }
    
    protected abstract void OnRemoveModifier();
}
