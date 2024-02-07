using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK;
using Zenject;

public class StackMoveWiggleEffect : StackWiggleEffect
{
    [SerializeField] private AnimationCurve _wiggleCurve;
    [SerializeField] private Vector3 _wiggleAxis;
    [SerializeField, Range(-1, 0)] private float _returnCoefficient;
    [SerializeField] private float _maxAmplitude;
    [SerializeField] private float _moveDuration;
    
    [SerializeField] private float _returnDuration;
    [SerializeField] private float _returnYoyoDuration;
    
    [Inject] private InputHandler _inputHandler;

    private float _coefficient = 0;
    
    public StackSettings Settings => StackSettings.Value;
    

    private void OnEnable()
    {
        _inputHandler.OnStartMove += OnStartMove;
        _inputHandler.OnStopMove += OnStopMove;
    }
    
    private void OnStartMove()
    {
        DOTween.Kill(this);
        CreateTweener(0, 1, _moveDuration);
    }

    private void OnStopMove()
    {
        DOTween.Kill(this);
        var sequence = DOTween.Sequence();
        sequence
            .Append(CreateTweener(_coefficient, _returnCoefficient, _returnDuration)).SetEase(Ease.OutBounce)
            .Append(CreateTweener(_returnCoefficient, 0, _returnYoyoDuration)).SetEase(Ease.OutSine)
            .SetId(this);
    }

    private Tweener CreateTweener(float from, float to, float duration)
    {
        return DOVirtual.Float(from, to, duration, value => _coefficient = value).SetId(this);
    }
    
    public override Vector3 GetOffset(int row, int column)
    {
        float wiggleCoefficient = _wiggleCurve.Evaluate((float)row / Settings.MaxRowsCount) * _maxAmplitude * _coefficient;
        return _wiggleAxis * wiggleCoefficient;
    }
}
