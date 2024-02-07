using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class StackIdleWiggleEffect : StackWiggleEffect
{
    [SerializeField] private AnimationCurve _wiggleCurve;
    [SerializeField] private Vector3 _wiggleAxis;
    [SerializeField] private float _maxWiggleAmplitude;
    [SerializeField] private float _wiggleDuration;

    private float _currentCoefficient = 0;
    
    public StackSettings Settings => StackSettings.Value;

    public override Vector3 GetOffset(int row, int column)
    {
        float wiggleCoefficient = _wiggleCurve.Evaluate((float)row / Settings.MaxRowsCount) * _maxWiggleAmplitude * _currentCoefficient;
        return _wiggleAxis * wiggleCoefficient;
    }

    private void OnEnable()
    {
        DOVirtual.Float(-1, 1, _wiggleDuration, value => _currentCoefficient = value)
            .SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).SetId(this);
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
    }
}
