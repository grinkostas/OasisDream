using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using StaserSDK;

public class PulseAnimation : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _fromScale;
    [SerializeField] private float _toScale;

    private void OnEnable()
    {
        Restart();
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
    }

    [Button]
    private void Restart()
    {
        DOTween.Kill(this);
        transform.localScale = _fromScale * Vector3.one;
        transform.DOPulse(_fromScale, _toScale, _duration).SetLoops(-1).SetId(this);
    }
}
