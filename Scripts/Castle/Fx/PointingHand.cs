using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;


public class PointingHand : MonoBehaviour
{
    [Header("Hand")]
    [SerializeField] private CanvasGroup _handImage;
    [SerializeField] private Vector3 _startRotation;
    [SerializeField] private Vector3 _clickRotation;
    [SerializeField] private Vector3 _clickScale;
    [Header("Move")]
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _destinationPoint;
    [SerializeField] private float _moveInTime;
    [SerializeField] private float _moveOutTime;
    [SerializeField] private Ease _moveInEase;
    [SerializeField] private Ease _moveOutEase;
    [Header("Click Fx")] 
    [SerializeField] private CanvasGroup _clickGlow;
    [SerializeField] private CanvasGroup _clickFx;
    [Header("Timings")]
    [SerializeField] private float _fadeTime = 0.25f;
    [SerializeField] private float _hideHandDelay;
    [Space]
    [SerializeField] private float _clickDelay = 0.15f;
    [SerializeField] private float _clickInDuration = 0.25f;
    [SerializeField] private float _clickOutDuration = 0.25f;
    [SerializeField] private float _delayMoveOut;
    [Space]
    [SerializeField] private float _clickFxZoomDuration = 0.25f;
    [SerializeField] private float _glowZoomDuration = 0.2f;
    [SerializeField] private float _glowFadeOutDuration = 0.2f;
    [Space] 
    [SerializeField] private float _repeatDelay;
    
    private Transform HandTransform => _handImage.transform;


    private void OnEnable()
    {
        Loop();
    }

    private void OnDisable()
    {
        DOTween.Kill(_repeatDelay);
        DOTween.Kill(this);
    }

    private void Loop()
    {
        Click();
        DOTween.Kill(_repeatDelay);
        DOVirtual.DelayedCall(_repeatDelay, Loop).SetId(_repeatDelay);
    }
    
    [NaughtyAttributes.Button]
    private void Click()
    {
        Init();
        
        ShowHand();
        ClickHand();
        ShowClickFx();
    }

    [NaughtyAttributes.Button]
    private void Init()
    {
        DOTween.Kill(this);
        InitHand();
        InitClickFx();
    }

    private void InitHand()
    {
        _handImage.alpha = 0.0f;
        HandTransform.localScale = Vector3.one;
        HandTransform.localRotation = Quaternion.Euler(_startRotation);
        HandTransform.position = _startPoint.position;
    }
    
    private void InitClickFx()
    {
        _clickGlow.alpha = 0.0f;
        _clickGlow.transform.localScale = Vector3.zero;
        _clickFx.alpha = 0.0f;
        _clickFx.transform.localScale = Vector3.zero;
    }
    
    private void ShowHand()
    {
        DOTween.Sequence().SetId(this)
            .Append(_handImage.transform.DOMove(_destinationPoint.position, _moveInTime).SetEase(_moveInEase))
            .Join(_handImage.DOFade(1, _fadeTime))
            .AppendInterval(_clickInDuration)
            .Append(_handImage.transform.DOMove(_startPoint.position, _moveOutTime).SetEase(_moveOutEase))
            .Join(_handImage.DOFade(0, _fadeTime).SetDelay(_hideHandDelay));
    }

    private void ClickHand()
    {
        DOTween.Sequence().SetId(this).SetDelay(_clickDelay)
            .Append(HandTransform.DOLocalRotate(_clickRotation, _clickInDuration).SetEase(Ease.OutCubic))
            .Join(HandTransform.DOScale(_clickScale, _clickInDuration).SetEase(Ease.OutCubic))
            .Append(HandTransform.DOLocalRotate(_startRotation, _clickOutDuration).SetEase(Ease.InCubic))
            .Join(HandTransform.DOScale(Vector3.one, _clickOutDuration).SetEase(Ease.InCubic));
    }

    private void ShowClickFx()
    {
        DOTween.Sequence().SetId(this).SetDelay(_clickDelay + _clickInDuration)
            .Append(_clickGlow.DOFade(1, _glowZoomDuration))
            .Join(_clickGlow.transform.DOScale(1, _glowZoomDuration).SetEase(Ease.OutSine))
            .Append(_clickGlow.DOFade(0, _glowFadeOutDuration));

        DOTween.Sequence().SetId(this).SetDelay(_clickDelay + _clickInDuration)
            .Append(_clickFx.DOFade(1, _clickFxZoomDuration))
            .Join(_clickFx.transform.DOScale(Vector3.one, _clickFxZoomDuration).SetEase(Ease.OutSine))
            .Append(_clickFx.DOFade(0, _clickFxZoomDuration));
    }
}
