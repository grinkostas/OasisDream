using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK;
using Zenject;

public class StackRotateWiggleEffect : StackWiggleEffect
{
    [SerializeField] private InstanceStack _instanceStack;
    [SerializeField] private AnimationCurve _wiggleCurve;
    [SerializeField] private float _minInputCoefficientToAffect;
    [SerializeField] private Vector3 _wiggleAxis;
    [SerializeField] private float _maxAmplitude;
    [SerializeField] private float _moveDuration;
    
    [SerializeField] private float _returnDuration;
    
    [Inject] private InputHandler _inputHandler;

    private float _coefficient = 0;
    private int _targetCoefficient = 0;
    public StackSettings Settings => StackSettings.Value;
    
    private void OnEnable()
    {
        _inputHandler.OnMove += OnMove;
        _inputHandler.OnStopMove += OnStopMove;
    }
    
    private void OnMove(Vector3 input)
    {
        float x = input.x;
        if(Mathf.Abs(x) < _minInputCoefficientToAffect)
            return;
        if (x > 0 && _targetCoefficient <= 0)
            Tween(1);
        else if (x < 0 && _targetCoefficient >= 0)
            Tween(-1);
    }

    private void Tween(int target)
    {
        DOTween.Kill(this);
        _targetCoefficient = target;
        CreateTweener(_coefficient, _targetCoefficient, _moveDuration).OnComplete(ReturnWiggle);
    }

    private void OnStopMove()
    {
        _targetCoefficient = 0;
        ReturnWiggle();
    }

    private void ReturnWiggle()
    {
        DOTween.Kill(this);
        CreateTweener(_coefficient, 0, _returnDuration);
    }

    private Tweener CreateTweener(float from, float to, float duration)
    {
        return DOVirtual.Float(from, to, duration, value => _coefficient = value).SetId(this);
    }
    
    public override Vector3 GetOffset(int row, int column)
    {
        int columnCount = Mathf.Max(Settings.MaxColumnsCount, _instanceStack.ItemsCount / Settings.MaxRowsCount);
        float wiggleCoefficient = _wiggleCurve.Evaluate((float)column / columnCount) * _maxAmplitude * _coefficient;
        return _wiggleAxis * wiggleCoefficient;
    }
}
