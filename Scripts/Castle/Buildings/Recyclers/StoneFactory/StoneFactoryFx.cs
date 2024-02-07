using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;

public class StoneFactoryFx : RecycleAdditionalFx
{
    [Header("Buggy")]
    [SerializeField] private SplinePositioner _splinePositioner;
    [SerializeField] private float _halfPathMoveTime;
    [SerializeField] private float _returnTime;
    [SerializeField] private float _stayTime;
    [Header("Hummer")] 
    [SerializeField] private Transform _hummer;
    [SerializeField] private Vector3 _localRotateAxis;
    [SerializeField] private float _rotateAnge;
    [SerializeField] private float _hummerHitTime;
    [SerializeField] private float _hummerStayTime;
    [SerializeField] private float _hummerReturnTime;
    [SerializeField] private ParticleSystem _hummerHitParticle;
    [Header("Items")]
    [SerializeField] private float _zoomTime;

    private float _moveProgress = 0.0f;

    private void OnValidate()
    {
        _halfPathMoveTime = (CycleDuration - _hummerHitTime - _stayTime - _hummerStayTime - _returnTime) / 2;
    }

    protected override void ProductCycle()
    {
        Restore();
        MoveToHummer();
        float delay = _halfPathMoveTime;
        DOVirtual.DelayedCall(delay, HummerHit).SetId(this);
        delay += _hummerStayTime;
        DOVirtual.DelayedCall(delay, EndCycle).SetId(this);
    }

    private void Restore()
    {
        DOTween.Kill(this);
        _moveProgress = 0.0f;
        _splinePositioner.SetPercent(0);
        HideItem(SourceItem);
        HideItem(ProductionItem);
    }
    
    private void MoveToHummer()
    {
        ShowItem(SourceItem, zoomTime:_zoomTime);
        MoveBuggy(0.5f, _halfPathMoveTime);
    }

    private void HummerHit()
    {
        _hummer.DOLocalRotate(_localRotateAxis * _rotateAnge, _hummerHitTime)
            .OnComplete(() =>
            {
                _hummerHitParticle.Play();
                HideItem(SourceItem);
                ShowItem(ProductionItem);
            });
        _hummer.DOLocalRotate(Vector3.zero, _hummerReturnTime).SetDelay(_hummerHitTime+_hummerStayTime).SetId(this);
    }

    private void EndCycle()
    {
        MoveBuggy(1.0f, _halfPathMoveTime).SetDelay(_stayTime)
            .OnComplete(()=>
            {
                HideItem(ProductionItem);
                MoveBuggy(0f, _returnTime).SetDelay(_stayTime).SetId(this);
            });
    }
    
   
    private void ShowItem(Transform item, float delay = 0, float zoomTime = 0)
    {
        DOTween.Kill(item);
        item.localPosition = Vector3.zero;
        item.localScale = Vector3.zero;
        item.gameObject.SetActive(true);
        item.DOScale(Vector3.one, zoomTime).SetEase(Ease.OutBack).SetDelay(0).SetId(this);
    }

    private void HideItem(Transform item)
    {
        DOTween.Kill(item);
        item.DOScale(Vector3.zero, _zoomTime)
            .OnComplete(()=>item.gameObject.SetActive(false)).SetId(this);
    }

    private float GetMoveProgress() => _moveProgress;

    private Tweener MoveBuggy(float endValue, float duration)
    {
        return DOTween.To(GetMoveProgress, value =>
        {
            _moveProgress = value;
            _splinePositioner.SetPercent(value);
        }, endValue, duration).SetId(this);
    }
}
