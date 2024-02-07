using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;

public class LoopYoyoTween : MonoBehaviour
{
    [SerializeField] private float _cycleDuration;
    [SerializeField] private Ease _cycleEase = Ease.OutQuad;

    private float _value = 0;
    public float Value
    {
        get => _value;
        private set
        {
            _value = value;
            OnValueChanged?.Invoke(value);
        } }
    public UnityAction<float> OnValueChanged { get; set; }
    

    private void OnEnable()
    {
        DOVirtual.Float(0, 1, _cycleDuration, value => Value = value).SetLoops(-1, LoopType.Yoyo).SetEase(_cycleEase)
            .SetId(this);
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
    }
}
